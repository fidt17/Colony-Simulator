using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpileState : CommandInputState {

    protected virtual string ColorHex => "#26FF56";
    protected fidt17.Utils.IntRectangle _selectionArea;
    protected Transform _area;

    public override void ExitState() {
        base.ExitState();
        if (_area != null) {
            GameObject.Destroy(_area.gameObject);
        }
    }

    protected override void SubscribeToEvents() {
        InputListener.GetInstance().OnMouse0_Down += OnLeftMouseDown;
        InputListener.GetInstance().OnMouse0_Up   += OnLeftMouseUp;
        InputListener.GetInstance().OnMouse1_Down += SwitchToDefaultState;

        SelectionTracker.GetInstance().OnAreaChange += OnAreaChange;
    }
    
    protected override void UnsubscribeFromEvents() {
        InputListener.GetInstance().OnMouse0_Down -= OnLeftMouseDown;
        InputListener.GetInstance().OnMouse0_Up   -= OnLeftMouseUp;
        InputListener.GetInstance().OnMouse1_Down -= SwitchToDefaultState;

        SelectionTracker.GetInstance().OnAreaChange -= OnAreaChange;
    }

    protected override void SetupSelectionTracker() {
        SelectionSettings settings = new SelectionSettings();
        settings.shouldDrawArea = false;
        SelectionTracker.GetInstance().SetSettings(settings);
    }

    protected void OnLeftMouseDown() => SelectionTracker.GetInstance().OnLeftMouseButtonDown();
    protected void OnLeftMouseUp() => SelectionTracker.GetInstance().OnLeftMouseButtonUp();

    protected virtual void OnAreaChange(object source, EventArgs e) {
        if (source is SelectionTracker) {
            SelectionTracker.OnAreaChangeArgs args = e as SelectionTracker.OnAreaChangeArgs;
            if (_selectionArea?.CompareTo(args.rectangle) == false || _selectionArea == null) {
                _selectionArea = args.rectangle;
                DrawSelectionArea(args.startMousePosition, args.currentMousePosition);
            }

            if (args.dragEnded) {
                StockpileCreator.CreateStockpileOnTiles(GetTilesInArea());
                GameObject.Destroy(_area?.gameObject);
            }
        }
    }

    protected List<Tile> GetTilesInArea() {
        List<Tile> tiles = new List<Tile>();
        foreach (Vector2Int position in _selectionArea.GetPositions()) {
            Tile t = Utils.TileAt(position.x, position.y);
            if (t is null || !t.IsTraversable) {
                continue;
            }
            tiles.Add(t);
        }
        return tiles;
    }

    protected void DrawSelectionArea(Vector2 start, Vector2 end) {
        if (_area == null) {
            _area = Factory.Create("SelectionArea", new Vector2Int((int) start.x, (int) start.y)).GetComponent<Transform>();
            Color color = Color.white;
            ColorUtility.TryParseHtmlString(ColorHex, out color);
            color.a = 60.0f/255.0f;
            _area.GetChild(0).GetComponent<SpriteRenderer>().color = color;
        }
        start = Utils.WorldToGrid(start);
        end = Utils.WorldToGrid(end);
        var rect = new fidt17.Utils.IntRectangle(Utils.ToVector2Int(start), Utils.ToVector2Int(end));

        _area.position = new Vector3(rect.Start.x - 0.5f, rect.Start.y - 0.5f, 0);
        float width = rect.End.x - rect.Start.x + 1;
        float height = rect.End.y - rect.Start.y + 1;
        _area.localScale = new Vector3(width, height, 1); 
    }
}