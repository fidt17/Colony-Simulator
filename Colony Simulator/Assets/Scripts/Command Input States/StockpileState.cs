using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpileState : CommandInputState {

    protected List<Tile> _tiles = new List<Tile>();
    protected Color _stockpileColor = Color.grey;

    public override void ExitState() {
        base.ExitState();
        ResetTilesColor();
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
        SelectionTracker.OnAreaChangeArgs args = e as SelectionTracker.OnAreaChangeArgs;

        ResetTilesColor();
        _tiles = Utils.GetTilesInArea(args.start, args.end);
        FilterTiles();
        if (args.dragEnded) {
            StockpileCreator.CreateStockpileOnTiles(_tiles);
            ResetTilesColor();
            _tiles.Clear();
        } else {
            ResetTilesColor();
            ColorTiles();
        }
    }

    protected void FilterTiles() {
        for (int i = _tiles.Count - 1; i >= 0; i--) {
            if (!Utils.NodeAt(_tiles[i].position).isTraversable) {
                _tiles.RemoveAt(i);
            }
        }
    }

    protected void ColorTiles() => _tiles.ForEach(x => x.SetColor(_stockpileColor));
    protected void ResetTilesColor() => _tiles.ForEach(x => x.ResetColor());
}