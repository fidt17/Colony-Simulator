using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommandInputState {

    public CommandInputState() => Initialize();

    public virtual void ExitState() => UnsubscribeFromEvents();

    protected abstract void UnsubscribeFromEvents();

    protected abstract void SubscribeToEvents();

    protected virtual void SetupSelectionTracker() => SelectionTracker.GetInstance().SetSettings(new SelectionSettings());

    protected virtual void Initialize() {
        UpdateCursorTexture();
        SubscribeToEvents();
        SetupSelectionTracker();
    }

    protected virtual void UpdateCursorTexture() => CursorManager.Instance.SwitchTexture(CursorManager.Instance.defaultTexture);
    protected virtual void SwitchToDefaultState() => CommandInputStateMachine.SwitchCommandState(new DefaultInputState());
}