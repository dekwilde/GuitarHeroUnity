using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System.IO.Ports;
using UnityEngine.Events;
using UnityEngine.UI;

public class ArduinoSerial : MonoBehaviour
{
    SerialPort stream;
    string strReceived;
    string comPort;
    public ArduinoManager arduinoManager;
    //
    private void Start()
    {
        arduinoManager = GetComponent<ArduinoManager>();
        loadJSON();
    }

    // a configuração da porta COM é feita pelo json
    private void loadJSON()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath + "/", "config.json");
        if (File.Exists(filePath))
        {
            var dataAsJson = File.ReadAllText(filePath);
            var loadedData = JsonUtility.FromJson<DataJson>(dataAsJson);
            comPort = loadedData.COM;
            StartStream();
        }
    }

    void StartStream()
    {
        stream = new SerialPort(comPort, 19200);
        stream.Open(); //Open the Serial Stream.
        stream.ReadTimeout = 1;
        //InvokeRepeating("ReadData", .5f, .1f);
    }

    void Update()
    {
        if (stream.IsOpen)
        {
            try
            {
                ReadDataInt(stream.ReadByte());
            }
            catch (System.Exception) { }
        }
    }

   public void ReadDataInt(int code)
    {
        char receivedChar = (char)code;
        string receivedString = receivedChar.ToString();
        //texto.text = receivedString;
        arduinoManager.SetData(receivedString);
        //OnReceived.Invoke(receivedString);
        //Debug.Log("received: " + receivedString);
    }

    public void ReadData()
    {
        stream.DiscardInBuffer();
        string strReceived = stream.ReadLine();
        arduinoManager.SetData(strReceived);
        //OnReceived.Invoke(strReceived);
        Debug.Log("string" + strReceived);
    }

    public void SendData(string data)
    {
        Debug.Log(data);
        if (stream.IsOpen)
        {
            //stream.Write(data);
            /*
            byte value;
            if (Byte.TryParse(data, out value))
            {
                byte[] buffer = new byte[] { value };
                stream.Write(buffer, 0, 1);
            }
            else
            {
                Debug.LogWarning("Failed to convert data to byte: " + data);
            }
            */
            stream.WriteLine(data);
        } else {
            StartStream();
        }
    }
}

// Crie uma nova classe de evento para aceitar o argumento.
[System.Serializable]
public class ReceivedEvent : UnityEvent<CustomInput, bool> { 

}