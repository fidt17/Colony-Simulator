using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpileState : CommandInputState {

    protected List<Tile> _tiles = new List<Tile>();
    protected fidt17.Utils.IntRectangle _selectionArea;
    protected GameObject _area;
    protected Color _stockpileColor = Color.grey;

    public override void ExitState() {
        base.ExitState();
        ResetTilesColor();
        _tiles.Clear();
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
        SelectionSettings settings;
        settings.selectionMask = new List<System.Type>();
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
                GetTilesInArea();
                ResetTilesColor();
                ColorTiles();
            }

            if (args.dragEnded) {
                StockpileCreator.CreateStockpileOnTiles(_tiles);
                ResetTilesColor();
                _tiles.Clear();
            }
        }
    }

    protected void GetTilesInArea() {
        //Removing tiles that are not in the area
        for (int i = _tiles.Count - 1; i >= 0; i--) {
            Tile t = _tiles[i];
            if (_selectionArea.Contains(t.position) == false) {
                _tiles.RemoveAt(i);
            }
        }

        //Adding new tiles
        foreach (Vector2Int position in _selectionArea.GetPositions()) {
            Tile t = Utils.TileAt(position.x, position.y);
            if (t == null || t.isTraversable == false || _tiles.Contains(t)) {
                continue;
            }
            _tiles.Add(t);
        }
    }

    protected void ColorTiles() {
        List<PathNode> nodes = new List<PathNode>();
        _tiles.ForEach(x => nodes.Add(Utils.NodeAt(x.position)));
        List<GameObject> areas = MeshGenerator.GetInstance().GenerateOverlapAreaOverNodes(nodes, new Color(0, 0, 0, 0.1f));

        _area = areas[0];
        for (int i = 1; i < areas.Count; i++) {
            areas[i].transform.parent = _area.transform;
        }
    }

    protected void ResetTilesColor() => GameObject.Destroy(_area);        
}