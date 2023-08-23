using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoManager : MonoBehaviour
{
    public ReceivedEvent OnReceived;
    private CustomInput currentKey;
    public CustomInput[] customInput;

    void Start() {
        OnReceived.AddListener(SetKeyEvent);
        StartCoroutine(StartLights());
    }

    public void SetData(string code)
    {
        FindInput(code);
        
        if(currentKey.positiveButtonKey == code)
        {
            OnReceived.Invoke(currentKey, true);
            //GetComponent<ArduinoSerial>().SendData(currentKey.negativeLightKey.ToString());
        } 
        else if(currentKey.negativeButtonKey == code)
        {
            OnReceived.Invoke(currentKey, false);
            // GetComponent<ArduinoSerial>().SendData(currentKey.positiveLightKey.ToString());
        }
        //currentKey = null;
    }

    void FindInput(string code){
        foreach (CustomInput input in customInput) {
            if(input.positiveButtonKey == code){
                currentKey = input;
            }
            if(input.negativeButtonKey == code){
                currentKey = input;
            }
        }
    }

    public void SetKeyEvent(CustomInput input, bool p)
    {
        if(input != null) {
            input.PressButton(p);
        }
    }

    public void LightsTest() {
        StartCoroutine(LightLoopOn());   
    }

    private IEnumerator LightLoopOn()
    {   
        int count = 0;
        while(count < customInput.Length) {
            yield return new WaitForSeconds(0.1f);
            GetComponent<ArduinoSerial>().SendData(customInput[count].negativeLightKey.ToString());
            count++;
            if(count >= customInput.Length) {
                Debug.Log("Desligando...");
                StartCoroutine(LightLoopOf()); 
            }
        }
    }

    private IEnumerator StartLights()
    {   
        yield return new WaitForSeconds(1);
        LightsTest();
    }

    private IEnumerator LightLoopOf()
    {   
        int count = 0;
        while(count < customInput.Length) {
            yield return new WaitForSeconds(0.1f);
            GetComponent<ArduinoSerial>().SendData(customInput[count].positiveLightKey.ToString());
            count++;
        }
    }

    public void SendData(string code) {
        GetComponent<ArduinoSerial>().SendData(code);
    }
}

// Classe do input personalizado
[System.Serializable]
public class CustomInput{
    [Header("Key Name")]
    [Space(5)]
    public string keyName;
    [Header("Key Codes")]
    [Space(5)]
    public string positiveButtonKey;
    [Space(5)]
    public string negativeButtonKey;
    [Space(10)]
    [Header("Lights Codes")]
    [Space(5)]
    public string positiveLightKey;
    [Space(5)]
    public string negativeLightKey;
    [Space(10)]

    public bool lightOn = true;
    public bool lightOff = true;

    [Header("Keyboard Button")]
    [Space(5)]
    public KeyCode keyCode;
    public bool isPressed;

    public void PressButton(bool p){
        if(p) {
            isPressed = true;
        } else {
            isPressed = false;
        }
    }
}
