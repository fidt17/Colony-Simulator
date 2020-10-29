using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SelectionTracker : Singleton<SelectionTracker> {
    
    public delegate void SelectionHandler();
    public event SelectionHandler OnSelect;

    public delegate void DragHandler(List<SelectableComponent> s);
    public event DragHandler OnDrag;

    public event EventHandler OnAreaChange;
    public class OnAreaChangeArgs : EventArgs {
        public fidt17.Utils.IntRectangle rectangle;
        public Vector2 startMousePosition;
        public Vector2 currentMousePosition;
        public bool dragEnded;
    }

    public void SetSettings(SelectionSettings newSettings) => _settings = newSettings;

    private SelectionSettings _settings;

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

    public List<SelectableComponent> GetSelectableInArea(Vector2 start, Vector2 end) {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(start, end);
        List<SelectableComponent> selectable = new List<SelectableComponent>();
        foreach (Collider2D collider in colliders) {
            SelectableComponent sc = collider.gameObject.GetComponent<SelectableComponent>();
            if (sc is null || !_settings.selectionMask.Contains(sc.selectable.GetType())) {
                continue;
            }
            selectable.Add(sc);
        }
        return selectable;
    }

    private void LateUpdate() {

        if (_lmbPressed == false && OnAreaChange != null) {
            OnAreaChangeArgs e = new OnAreaChangeArgs();
            Vector2Int cursorCoords = Utils.CursorToCoordinates();
            e.rectangle = new fidt17.Utils.IntRectangle(cursorCoords, cursorCoords);
            e.startMousePosition = cursorCoords;
            e.currentMousePosition = cursorCoords;
            e.dragEnded = false;
            OnAreaChange.Invoke(this, e);
        }
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
            
            if (_settings.shouldDrawArea) {
                DrawSelectionArea(startMousePosition, currentMousePosition);
            }

            if (OnDrag != null) {
                List<SelectableComponent> selectableInArea = GetSelectableInArea(startMousePosition, currentMousePosition);
                OnDrag.Invoke(selectableInArea);
            }

            if (OnAreaChange != null) {
                OnAreaChangeArgs e = new OnAreaChangeArgs();
                e.rectangle = new fidt17.Utils.IntRectangle(Utils.WorldToGrid(startMousePosition), Utils.WorldToGrid(currentMousePosition));
                e.startMousePosition = startMousePosition;
                e.currentMousePosition = currentMousePosition;
                e.dragEnded = false;
                OnAreaChange.Invoke(this, e);
            }
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
        DeselectEverything();
        GetSelectableInArea(start, end).ForEach(x => Select(x));
        ResetSelectionArea();
        OnSelect?.Invoke();

        if (OnAreaChange != null) {
                OnAreaChangeArgs e = new OnAreaChangeArgs();
                e.rectangle = new fidt17.Utils.IntRectangle(Utils.WorldToGrid(start), Utils.WorldToGrid(end));
                e.dragEnded = true;
                OnAreaChange.Invoke(this, e);
        }
    }
}

public class SelectionSettings {
    public List<Type> selectionMask = new List<Type>();
    public bool shouldDrawArea = true;
}