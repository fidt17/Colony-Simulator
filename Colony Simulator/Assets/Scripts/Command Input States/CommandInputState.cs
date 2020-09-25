using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommandInputState {

    public CommandInputState() => Initialize();

    public virtual void Initialize() {
        UpdateCursorTexture();
        SubscribeToEvents();
    }

    public virtual void UpdateCursorTexture() => CursorManager.Instance.SwitchTexture(CursorManager.Instance.defaultTexture);

    public abstract void SubscribeToEvents();
    public abstract void UnsubscribeFromEvents();
}