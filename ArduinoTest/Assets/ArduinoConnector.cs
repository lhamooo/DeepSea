using UnityEngine;
using System;
using System.IO.Ports;
using System.Threading;

public class NewMonoBehaviourScript : MonoBehaviour
{
    SerialPort serialPort = new SerialPort("/dev/cu.usbmodem101", 9600);

    float lastY;
    float rotSpeed;

    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();

        serialPort.ReadTimeout = 50;
        try
        {
            serialPort.Open();
            Thread.Sleep(1200);
            serialPort.DiscardInBuffer();
        }
        catch (Exception e)
        {
            Debug.LogError("Port open failed: " + e.Message);
        }

        lastY = transform.eulerAngles.y;
    }

    void Update()
    {
        if (serialPort == null || !serialPort.IsOpen) return;

        if (serialPort.BytesToRead > 0)
        {
            string data = serialPort.ReadLine().Trim();

            if (int.TryParse(data, out int value))
            {
                float y = value * 360f / 1023f;

                transform.rotation = Quaternion.Euler(0f, y, 0f);

                float currentY = transform.eulerAngles.y;

                float delta = Mathf.DeltaAngle(lastY, currentY);
                rotSpeed = Mathf.Abs(delta) / Time.deltaTime;

                lastY = currentY;


                float t = Mathf.InverseLerp(0f, 500f, rotSpeed);

                if (rend != null)
                    rend.material.color = Color.Lerp(Color.blue, Color.red, t);

                float s = Mathf.Lerp(1f, 1.2f, t);
                transform.localScale = new Vector3(s, s, s);
            }
        }
    }

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen) serialPort.Close();
    }
}
