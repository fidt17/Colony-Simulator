using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildCommandInputState : CommandInputState {

    private Dictionary<Tile, GameObject> _tiles = new Dictionary<Tile, GameObject>();
    private fidt17.Utils.IntRectangle _selectionArea;
    private Stack<GameObject> _jobIconsPool = new Stack<GameObject>();
    private ConstructionScriptableObject _constructionData;

    public BuildCommandInputState(ConstructionScriptableObject constructionData) : base() {
        _constructionData = constructionData;
    }

    public override void ExitState() {
        base.ExitState();

        Dictionary<Tile, GameObject> temp = new Dictionary<Tile, GameObject>(_tiles);
        foreach (KeyValuePair<Tile, GameObject> pair in temp) {
            _jobIconsPool.Push(pair.Value);
            _tiles.Remove(pair.Key);
        }
        temp.Clear();

        while (_jobIconsPool.Count != 0) {
            GameObject.Destroy(_jobIconsPool.Pop());
        }
    }

    protected override void SubscribeToEvents() {
        InputListener.GetInstance().OnMouse0_Down += OnLeftClickDown;
        InputListener.GetInstance().OnMouse0_Up   += OnLeftClickUp;
        InputListener.GetInstance().OnMouse1_Down += SwitchToDefaultState;
        InputListener.GetInstance().OnEscape_Down += SwitchToDefaultState;

        SelectionTracker.GetInstance().OnAreaChange += OnAreaChange;
    }

    protected override void UnsubscribeFromEvents() {
        InputListener.GetInstance().OnMouse0_Down -= OnLeftClickDown;
        InputListener.GetInstance().OnMouse0_Up   -= OnLeftClickUp;
        InputListener.GetInstance().OnMouse1_Down -= SwitchToDefaultState;
        InputListener.GetInstance().OnEscape_Down -= SwitchToDefaultState;

        SelectionTracker.GetInstance().OnAreaChange -= OnAreaChange;
    }

    protected override void SetupSelectionTracker() {
        SelectionSettings settings = new SelectionSettings();
        settings.shouldDrawArea = false;
        SelectionTracker.GetInstance().SetSettings(settings);
    }

    protected override void UpdateCursorTexture() => CursorManager.Instance.SwitchTexture(CursorManager.Instance.cutStateTexture);

    private void OnLeftClickDown() => SelectionTracker.GetInstance().OnLeftMouseButtonDown();
    private void OnLeftClickUp()   => SelectionTracker.GetInstance().OnLeftMouseButtonUp();

    protected virtual void OnAreaChange(object source, EventArgs e) {
        if (source is SelectionTracker) {
            SelectionTracker.OnAreaChangeArgs args = e as SelectionTracker.OnAreaChangeArgs;
            Vector2Int startPosition = Utils.WorldToGrid(args.startMousePosition);
            Vector2Int currentPosition = Utils.WorldToGrid(args.currentMousePosition);

            int width = Mathf.Abs(currentPosition.x - startPosition.x) + 1;
            int height = Mathf.Abs(currentPosition.y - startPosition.y) + 1;

            Vector2Int start = startPosition;
            fidt17.Utils.IntRectangle rect;

            if (width >= height) {
                Vector2Int end = new Vector2Int(currentPosition.x, startPosition.y);
                rect = new fidt17.Utils.IntRectangle(start, end);
            } else {
                Vector2Int end = new Vector2Int(startPosition.x, currentPosition.y);
                rect = new fidt17.Utils.IntRectangle(start, end);
            }

            if (args.dragEnded) {
                CreateJobs();
            }

            if (_selectionArea?.CompareTo(rect) == false || _selectionArea == null) {
                _selectionArea = rect;
                GetNewPlans();
            }
        }
    }

    private void GetNewPlans() {
        //Removing non-valid plans from existing dictionary
        Dictionary<Tile, GameObject> temp = new Dictionary<Tile, GameObject>(_tiles);
        foreach (KeyValuePair<Tile, GameObject> pair in temp) {
            if (_selectionArea.Contains(pair.Key.position) == false) {
                pair.Value.SetActive(false);
                _jobIconsPool.Push(pair.Value);
                _tiles.Remove(pair.Key);
            }
        }
        temp.Clear();

        //Detecting new tiles where we can create a plan.
        foreach (Vector2Int position in _selectionArea.GetPositions()) {
            Tile t = Utils.TileAt(position.x, position.y);
            if ((t is null)
            || _tiles.ContainsKey(t)
            || (t.content?.StaticObject is null && t.isTraversable == false)){
                continue;
            }

            //(t.content?.staticObject != null && t.content.staticObject.GetType() != typeof(Vegetation))

            _tiles.Add(t, CreateJobIcon(t));
        }
    }

    private GameObject CreateJobIcon(Tile tile) {
       if (_jobIconsPool.Count != 0) {
           GameObject jobIcon = _jobIconsPool.Pop();
           jobIcon.transform.position = Utils.ToVector3(tile.position);
           jobIcon.SetActive(true);
           return jobIcon;
       }
       GameObject obj = GameObject.Instantiate(_constructionData.planPrefab);
       obj.transform.position = Utils.ToVector3(tile.position);
       obj.SetActive(true);
       return obj;
    }

    private void CreateJobs() {
        Dictionary<Tile, GameObject> temp = new Dictionary<Tile, GameObject>(_tiles);
        foreach(KeyValuePair<Tile, GameObject> pair in temp) {
            ConstructionPlan plan = new ConstructionPlan();
            plan.SetData(_constructionData, pair.Key.position);
            plan.SetGameObject(pair.Value);
            _tiles.Remove(pair.Key);
        }
    }
}