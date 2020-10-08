using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputListener : Singleton<InputListener> {

    public delegate void OnButtonPressed();
    
    public event OnButtonPressed OnEscape_Down;
    public event OnButtonPressed OnTab_Up;

    public event OnButtonPressed OnW_Pressed;
    public event OnButtonPressed OnA_Pressed;
    public event OnButtonPressed OnS_Pressed;
    public event OnButtonPressed OnD_Pressed;

    public event OnButtonPressed OnMouse0_Pressed;
    public event OnButtonPressed OnMouse0_Down;
    public event OnButtonPressed OnMouse0_Up;
    public event OnButtonPressed OnMouse1_Down;

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            OnEscape_Down?.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.Tab)) {
            OnTab_Up?.Invoke();
        }

        if (Input.GetKey(KeyCode.W)) {
            OnW_Pressed?.Invoke();
        }

        if (Input.GetKey(KeyCode.A)) {
            OnA_Pressed?.Invoke();
        }

        if (Input.GetKey(KeyCode.S)) {
            OnS_Pressed?.Invoke();
        }

        if (Input.GetKey(KeyCode.D)) {
            OnD_Pressed?.Invoke();
        }

        if (Input.GetMouseButton(0)) {
            OnMouse0_Pressed?.Invoke();
        }

        if (Input.GetMouseButtonDown(0)) {
            OnMouse0_Down?.Invoke();
        }

        if (Input.GetMouseButtonUp(0)) {
            OnMouse0_Up?.Invoke();
        }

        if (Input.GetMouseButtonDown(1)) {
            OnMouse1_Down?.Invoke();
        }
    }
}