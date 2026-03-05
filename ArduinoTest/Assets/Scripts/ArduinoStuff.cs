using UnityEngine;
using System.IO.Ports;
using System.Threading;

[System.Serializable]
public class ArduinoResult
{
    public string geschwindigkeit;
    public int leds;
}

public class ArduinoStuff : MonoBehaviour
{
    SerialPort serialPort = new SerialPort("/dev/cu.usbmodem101", 9600);
    private InteractionRadius interactionZone;

    void Start()
    {
        interactionZone = GetComponent<InteractionRadius>();

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
                interactionZone.StartInteraction(result);
        }
    }

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();
    }
}