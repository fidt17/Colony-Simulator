using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DefaultInputState : CommandInputState {
    
    public void OnLeftMouseButtonDown() => SelectionTracker.GetInstance().OnLeftMouseButtonDown();

    protected override void SubscribeToEvents() {
        InputController.GetInstance().OnMouse0_Down += OnLeftMouseButtonDown;
        InputController.GetInstance().OnMouse0_Up   += OnLeftMouseButtonUp;
    }

    protected override void UnsubscribeFromEvents() {
        InputController.GetInstance().OnMouse0_Down -= OnLeftMouseButtonDown;
        InputController.GetInstance().OnMouse0_Up   -= OnLeftMouseButtonUp;
    } 

    protected override  void SetUpSelectionMask() {
        List<Type> selectionMask = new List<Type>();
        selectionMask.Add(typeof(Human));
        SelectionTracker.GetInstance().selectionMask = selectionMask;
    }

    private void OnLeftMouseButtonUp() => SelectionTracker.GetInstance().OnLeftMouseButtonUp();
}