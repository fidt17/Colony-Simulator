using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//TODO verify that all selected objects are of one type

public class SelectionController : MonoBehaviour {

    public static SelectionController Instance;

    private List<SelectableComponent> _selected = new List<SelectableComponent>();

    private Vector2 _currMousePosition;

    private void Awake() {

        if (Instance != null) {
            Debug.Log("Only one SelectionController can exist at a time.");
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void LateUpdate() {

        _currMousePosition = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);

        CheckLeftClick();
    }

    public List<SelectableComponent> GetSelectedColonists() {
        
        if (_selected.Count == 0)
            return null;

        if (_selected[0].selectable is Human)
            return _selected;
        
        return null;
    }

    private void CheckLeftClick() {

        if (Input.GetMouseButtonDown(0)) {

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            DeselectEverything();

            RaycastHit2D hit = Physics2D.Raycast(_currMousePosition, Vector2.zero);
            if (hit.collider != null)
                ProcessSelection(hit.collider.gameObject.GetComponent<SelectableComponent>());
            else
                CommandInput.Instance.SwitchCommand(new EmptyCommandInputMode());
        }
    }

    private void ProcessSelection(SelectableComponent selected) {

        if (selected == null)
            return;

        selected.Select();
        _selected.Add(selected);

        if (selected.selectable is Character) {

            UIManager.Instance.OpenCharacterWindow(selected.selectable as Character);

            if (selected.selectable is Human)
                CommandInput.Instance.SwitchCommand(new MoveCommandInputMode());
        }
    }

    private void DeselectEverything() {

        foreach (SelectableComponent s in _selected)
            s.Deselect();
        
        UIManager.Instance.CloseCharacterWindow();
        _selected = new List<SelectableComponent>();
    }
}
