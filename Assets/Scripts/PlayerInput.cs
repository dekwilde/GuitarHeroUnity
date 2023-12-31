﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInput
{
    public int deviceId;
    public Device device;
    public bool[] fred;
    public bool[] fredState;
    public bool strumPressed;
    public bool startPressed;
    public bool starPressed;
    public float tilt,
        whammy;

    public enum Device
    {
        Keyboard,
        Arduino,
        Xinput
    }

    public ArduinoManager arduinoManager;

    public PlayerInput(Device _device, int _deviceId, ArduinoManager _arduinoManager)
    {
        device = _device;
        deviceId = _deviceId;
        fred = new bool[5];
        fredState = new bool[5];
        arduinoManager = _arduinoManager;
    }

    void Start() {
        
    }

    public void Update()
    {
        if (device == Device.Xinput)
        {
            fred[0] = XInput.GetButton(deviceId, XInput.Button.A);
            fred[1] = XInput.GetButton(deviceId, XInput.Button.B);
            fred[2] = XInput.GetButton(deviceId, XInput.Button.Y);
            fred[3] = XInput.GetButton(deviceId, XInput.Button.X);
            fred[4] = XInput.GetButton(deviceId, XInput.Button.LB);
            startPressed = XInput.GetButtonDown(deviceId, XInput.Button.Start);
            starPressed = XInput.GetButtonDown(deviceId, XInput.Button.Back);
            strumPressed =
                XInput.GetButtonDown(deviceId, XInput.Button.DPadDown)
                | XInput.GetButtonDown(deviceId, XInput.Button.DPadUp);
            tilt = XInput.GetAxis(deviceId, XInput.Axis.RY);
            whammy = XInput.GetAxis(deviceId, XInput.Axis.RX);
        }

        else if (device == Device.Arduino)
        {
            UpdateArduinoFredState(arduinoManager.customInput[0], 0);
            UpdateArduinoFredState(arduinoManager.customInput[1], 1);
            UpdateArduinoFredState(arduinoManager.customInput[2], 2);
            UpdateArduinoFredState(arduinoManager.customInput[3], 3);
            UpdateArduinoFredState(arduinoManager.customInput[4], 4);

            startPressed = Input.GetKeyDown(KeyCode.S);
            starPressed = Input.GetKeyDown(KeyCode.A);
            strumPressed = true;
            tilt = 1f; // Not applicable for keyboard input
            whammy = 1f; // Not applicable for keyboard input
        }

        else if (device == Device.Keyboard)
        {
            UpdateFredState(KeyCode.Alpha1, 0);
            UpdateFredState(KeyCode.Alpha2, 1);
            UpdateFredState(KeyCode.Alpha3, 2);
            UpdateFredState(KeyCode.Alpha4, 3);
            UpdateFredState(KeyCode.Alpha5, 4);

            startPressed = Input.GetKeyDown(KeyCode.S);
            starPressed = Input.GetKeyDown(KeyCode.A);
            strumPressed = true;
            tilt = 1f; // Not applicable for keyboard input
            whammy = 1f; // Not applicable for keyboard input
        }
    }

    private void UpdateFredState(KeyCode key, int index)
    {
        if (Input.GetKeyDown(key))
        {
            fredState[index] = true;
        }
        if (Input.GetKeyUp(key))
        {
            fredState[index] = false;
        }

        fred[index] = fredState[index];
    }

    private void UpdateArduinoFredState(CustomInput input, int index)
    {
        if (input.isPressed)
        {
            fredState[index] = true;
        }
        else
        {
            fredState[index] = false;
        }

        fred[index] = fredState[index];
    }
}
