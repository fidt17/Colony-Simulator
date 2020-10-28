using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DefaultInputState : CommandInputState {
    
    public void OnLeftMouseButtonDown() => SelectionTracker.GetInstance().OnLeftMouseButtonDown();

    protected override void SubscribeToEvents() {
        InputListener.GetInstance().OnMouse0_Down += OnLeftMouseButtonDown;
        InputListener.GetInstance().OnMouse0_Up   += OnLeftMouseButtonUp;
    }

    protected override void UnsubscribeFromEvents() {
        InputListener.GetInstance().OnMouse0_Down -= OnLeftMouseButtonDown;
        InputListener.GetInstance().OnMouse0_Up   -= OnLeftMouseButtonUp;
    } 

    protected override  void SetupSelectionTracker() {
        SelectionSettings settings = new SelectionSettings();
        settings.selectionMask = new List<System.Type>() {
            typeof(Human)
        };
        settings.shouldDrawArea = true;
        SelectionTracker.GetInstance().SetSettings(settings);
    }

    private void OnLeftMouseButtonUp() => SelectionTracker.GetInstance().OnLeftMouseButtonUp();
}