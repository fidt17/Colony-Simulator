using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommandInputStateMachine {

    public static CommandInputState currentCommandState;

    public static void Initialize() => SwitchCommandState(new DefaultInputState());
    
    public static void SwitchCommandState(CommandInputState newCommandState) {
        if (currentCommandState == newCommandState) {
            return;
        }
        currentCommandState?.ExitState();
        currentCommandState = newCommandState;
    }    
}