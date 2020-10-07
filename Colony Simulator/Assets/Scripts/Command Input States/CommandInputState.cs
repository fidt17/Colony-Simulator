using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommandInputState {

    public CommandInputState() => Initialize();

    public virtual void ExitState() => UnsubscribeFromEvents();

    protected abstract void UnsubscribeFromEvents();

    protected abstract void SubscribeToEvents();

    protected abstract void SetUpSelectionMask();

    protected virtual void Initialize() {
        UpdateCursorTexture();
        SubscribeToEvents();
        SetUpSelectionMask();
    }

    protected virtual void UpdateCursorTexture() => CursorManager.Instance.SwitchTexture(CursorManager.Instance.defaultTexture);
    protected virtual void SwitchToDefaultState() => CommandInputStateMachine.SwitchCommandState(new DefaultInputState());
}