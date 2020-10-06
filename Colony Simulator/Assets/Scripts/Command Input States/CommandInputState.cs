using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommandInputState {

    public CommandInputState() => Initialize();

    public abstract void UnsubscribeFromEvents();

    protected abstract void SubscribeToEvents();

    protected virtual void Initialize() {
        UpdateCursorTexture();
        SubscribeToEvents();
    }

    protected virtual void UpdateCursorTexture() => CursorManager.Instance.SwitchTexture(CursorManager.Instance.defaultTexture);
}