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
        ResetLights();
    }

    public void ResetLights() {
        foreach (CustomInput input in customInput) {
            GetComponent<ArduinoSerial>().SendData(input.positiveButtonKey.ToString());
        }
    }

    public void SetData(string code)
    {
        FindInput(code);
        
        if(currentKey.positiveButtonKey == code)
        {
            OnReceived.Invoke(currentKey, true);
            GetComponent<ArduinoSerial>().SendData(currentKey.negativeButtonKey.ToString());
        } 
        else if(currentKey.negativeButtonKey == code)
        {
            Debug.Log("foi");
            OnReceived.Invoke(currentKey, false);
            GetComponent<ArduinoSerial>().SendData(currentKey.positiveButtonKey.ToString());
        }
    }

    void FindInput(string code){
        foreach (CustomInput input in customInput) {
            if(input.positiveButtonKey == code || input.negativeButtonKey == code){
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
