using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputController : MonoBehaviour {

    public static InputController Instance;

    public delegate void OnButtonPressed();
    public event OnButtonPressed OnEscape_Down;
    public event OnButtonPressed OnW_Pressed;
    public event OnButtonPressed OnA_Pressed;
    public event OnButtonPressed OnS_Pressed;
    public event OnButtonPressed OnD_Pressed;

    private void Awake() {

        if (Instance != null) {

            Debug.LogError("Only one InputController can exist at a time.");
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Escape))
            OnEscape_Down?.Invoke();

        if (Input.GetKey(KeyCode.W))
            OnW_Pressed?.Invoke();

        if (Input.GetKey(KeyCode.A))
            OnA_Pressed?.Invoke();

        if (Input.GetKey(KeyCode.S))
            OnS_Pressed?.Invoke();

        if (Input.GetKey(KeyCode.D))
            OnD_Pressed?.Invoke();
    }
}