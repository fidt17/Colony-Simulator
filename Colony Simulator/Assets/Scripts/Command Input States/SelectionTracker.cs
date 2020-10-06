using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SelectionTracker : Singleton<SelectionTracker> {
    
    public List<Human> Colonists {
        get {
            if (_selected.Count == 0 || _selected[0].selectable.GetType() != typeof(Human)) {
                return null;
            }

            List<Human> colonists = new List<Human>();
            _selected.ForEach(x => colonists.Add(x.selectable as Human));
            return colonists;
        }
    }

    public List<Tree> Trees {
        get {
            return null;
        }
    }

    [SerializeField] private Transform _selectionArea = null;
    private List<SelectableComponent> _selected = new List<SelectableComponent>();
    private bool _lmbPressed = false;

    protected override void Awake() {
        if (_selectionArea == null) {
            Debug.LogError("Selection area gameobject is not linked.", this);
        }
        ResetSelectionArea();
    }

    public void OnLeftMouseButtonDown() => StartCoroutine(StartSelectionAreaDrag());

    public void OnLeftMouseButtonUp() {
        _lmbPressed = false;
    }

    public void SingleClickSelection() {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast((Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        SelectableComponent sc = hit.collider?.gameObject.GetComponent<SelectableComponent>();

        if (sc != null) {
            Select(sc);
        } else {
            DeselectEverything();
        }
    }

    public void Select(SelectableComponent selectableComponent) {
        _selected.Add(selectableComponent);
        selectableComponent.Select();
    }

    public void DeselectEverything() {
        foreach (SelectableComponent s in _selected) {
            s.Deselect();
        }
        _selected.Clear();
    }

    private IEnumerator StartSelectionAreaDrag() {
        _lmbPressed = true;

        Vector2 startMousePosition = Utils.CursorToWorldPosition();
        Vector2 currentMousePosition = startMousePosition;

        while (_lmbPressed) {
            currentMousePosition = Utils.CursorToWorldPosition();
            DrawSelectionArea(startMousePosition, currentMousePosition);
            yield return null;
        }

        ProcessSelectionArea(startMousePosition, currentMousePosition);
    }

    private void DrawSelectionArea(Vector2 start, Vector2 end) {
        _selectionArea.transform.position = new Vector3(start.x, start.y, 0);
        float width  = end.x - start.x;
        float height = end.y - start.y;
        _selectionArea.transform.localScale = new Vector3(width, height, 1); 
    }

    private void ResetSelectionArea() => _selectionArea.transform.localScale = Vector3.zero;

    private void ProcessSelectionArea(Vector2 start, Vector2 end) {
        List<Type> selectionMask = new List<Type>();
        selectionMask.Add(typeof(Human));

        Collider2D[] colliders = Physics2D.OverlapAreaAll(start, end);

        DeselectEverything();
        foreach (Collider2D collider in colliders) {
            SelectableComponent sc = collider.gameObject.GetComponent<SelectableComponent>();
            if (sc is null) {
                continue;
            }

            if (!selectionMask.Contains(sc.selectable.GetType())) {
                continue;
            }

            Select(sc);            
        }
        
        ResetSelectionArea();
    }
}