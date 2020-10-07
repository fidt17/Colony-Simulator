using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SelectionTracker : Singleton<SelectionTracker> {
    
    public delegate void SelectionHandler();
    public event SelectionHandler OnSelect;

    public List<Type> selectionMask = new List<Type>();

    public List<T> GetSelected<T>() {
        List<T> selected = new List<T>();
        foreach (SelectableComponent sc in _selected) {
            if (sc.selectable.GetType() == typeof(T)) {
                selected.Add((T)sc.selectable);
            }
        }
        return selected;
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
    public void OnLeftMouseButtonUp() => _lmbPressed = false;

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
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
            yield break;
        }

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
        Collider2D[] colliders = Physics2D.OverlapAreaAll(start, end);
        DeselectEverything();
        foreach (Collider2D collider in colliders) {
            SelectableComponent sc = collider.gameObject.GetComponent<SelectableComponent>();
            if (sc is null || !selectionMask.Contains(sc.selectable.GetType())) {
                continue;
            }
            Select(sc);
        }
        ResetSelectionArea();

        OnSelect?.Invoke();
    }
}