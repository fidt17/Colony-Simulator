using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DefaultInputState : CommandInputState {
    
    private List<SelectableComponent> _selected = new List<SelectableComponent>();

    public override void SubscribeToEvents() => InputController.Instance.OnMouse0_Down += OnLeftMouseButtonDown;

    public override void UnsubscribeFromEvents() => InputController.Instance.OnMouse0_Down -= OnLeftMouseButtonDown;

    public void OnLeftMouseButtonDown() {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast((Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        SelectableComponent sc = hit.collider?.gameObject.GetComponent<SelectableComponent>();

        if (sc != null) {
            SelectionTracker.Select(sc);
        } else {
            SelectionTracker.DeselectEverything();
        }
    }
}