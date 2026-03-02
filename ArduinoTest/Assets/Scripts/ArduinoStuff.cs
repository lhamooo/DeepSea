using UnityEngine;
using System.IO.Ports;
using System.Threading;

[System.Serializable]
class ArduinoResult
{
    public string geschwindigkeit;
    public int leds;
}

public class ArduinoStuff : MonoBehaviour
{
    SerialPort serialPort = new SerialPort("/dev/cu.usbmodem101", 9600);

    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();

        serialPort.ReadTimeout = 50;
        serialPort.Open();
        Thread.Sleep(1200);
        serialPort.DiscardInBuffer();
    }

    void Update()
    {
        if (!serialPort.IsOpen)
            return;

        while (serialPort.BytesToRead > 0)
        {
            string data = serialPort.ReadLine().Trim();
            print("DATA: " + data);

            if (string.IsNullOrEmpty(data))
                continue;

            if (!data.StartsWith("{"))
                continue;

            ArduinoResult result = JsonUtility.FromJson<ArduinoResult>(data);

            if (result != null && !string.IsNullOrEmpty(result.geschwindigkeit))
                ApplyResult(result);
        }
    }

    void ApplyResult(ArduinoResult result)
    {
        Color c = Color.white;

        if (result.geschwindigkeit == "slow")
            c = Color.red;
        else if (result.geschwindigkeit == "middle")
            c = Color.blue;
        else if (result.geschwindigkeit == "fast")
            c = Color.green;

        rend.material.color = c;

        float t = Mathf.InverseLerp(0, 12, result.leds);
        float s = Mathf.Lerp(1f, 1.3f, t);
        transform.localScale = Vector3.one * s;
    }

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();
    }
}