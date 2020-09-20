using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInput : MonoBehaviour {

    public static CommandInput Instance;

    public CommandInputMode curretCommandMode = null;

    private void Awake() {

        if (Instance != null) {
            Debug.Log("Only one CommandInput can exist at a time.");
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void SwitchCommand(CommandInputMode newCommandMode) {

        if (curretCommandMode == newCommandMode)
            return;

        curretCommandMode?.UnsubscribeFromEvents();
        curretCommandMode = newCommandMode;
    }    
}