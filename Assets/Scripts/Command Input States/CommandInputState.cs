using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommandInputState {

    public CommandInputState() => Initialize();

    public virtual void ExitState() => UnsubscribeFromEvents();

    protected abstract void UnsubscribeFromEvents();

    protected abstract void SubscribeToEvents();

    protected virtual void SetupSelectionTracker() {
        SelectionSettings settings;
        settings.selectionMask = new List<System.Type>();
        settings.shouldDrawArea = true;
        SelectionTracker.GetInstance().SetSettings(settings);
    }

    protected virtual void Initialize() {
        UpdateCursorTexture();
        SubscribeToEvents();
        SetupSelectionTracker();
    }

    protected virtual void UpdateCursorTexture() => CursorManager.Instance.SwitchTexture(CursorManager.Instance.defaultTexture);
    protected virtual void SwitchToDefaultState() => CommandInputStateMachine.SwitchCommandState(new DefaultInputState());
}