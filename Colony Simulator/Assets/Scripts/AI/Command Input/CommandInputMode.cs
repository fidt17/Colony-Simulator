using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommandInputMode {

    public CommandInputMode() {

        SubscribeToEvents();
    }

    public abstract void SubscribeToEvents();
    public abstract void UnsubscribeFromEvents();
}
