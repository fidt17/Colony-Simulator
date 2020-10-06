using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DefaultInputState : CommandInputState {
    
    public override void UnsubscribeFromEvents() => InputController.GetInstance().OnMouse0_Down -= OnLeftMouseButtonDown;
    
    public void OnLeftMouseButtonDown() => SelectionTracker.GetInstance().OnLeftMouseButtonDown();

    protected override void SubscribeToEvents() {
        InputController.GetInstance().OnMouse0_Down += OnLeftMouseButtonDown;
        InputController.GetInstance().OnMouse0_Up   += OnLeftMouseButtonUp;
    } 

    private void OnLeftMouseButtonUp() => SelectionTracker.GetInstance().OnLeftMouseButtonUp();
}