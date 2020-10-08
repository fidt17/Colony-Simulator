using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpileState : CommandInputState {

    private List<Tile> _tiles = new List<Tile>();
    private Color _stockpileColor = Color.grey;

    public override void ExitState() {
        base.ExitState();
        ResetTilesColor();
    }

    protected override void SubscribeToEvents() {
        InputListener.GetInstance().OnMouse0_Down += OnLeftMouseDown;
        InputListener.GetInstance().OnMouse0_Up   += OnLeftMouseUp;
        InputListener.GetInstance().OnMouse1_Down += SwitchToDefaultState;

        SelectionTracker.GetInstance().OnSelect += OnDragStop;
        SelectionTracker.GetInstance().OnDrag   += OnDragUpdate;
    }
    
    protected override void UnsubscribeFromEvents() {
        InputListener.GetInstance().OnMouse0_Down -= OnLeftMouseDown;
        InputListener.GetInstance().OnMouse0_Up   -= OnLeftMouseUp;
        InputListener.GetInstance().OnMouse1_Down -= SwitchToDefaultState;

        SelectionTracker.GetInstance().OnSelect -= OnDragStop;
        SelectionTracker.GetInstance().OnDrag   -= OnDragUpdate;
    }

    protected override void SetupSelectionTracker() {
        SelectionSettings settings;
        settings.selectionMask = new List<System.Type>() {
            typeof(Tile)
        };
        settings.shouldDrawArea = false;
        SelectionTracker.GetInstance().SetSettings(settings);
    }

    private void OnLeftMouseDown() => SelectionTracker.GetInstance().OnLeftMouseButtonDown();
    private void OnLeftMouseUp() => SelectionTracker.GetInstance().OnLeftMouseButtonUp();

    private void OnDragUpdate(List<SelectableComponent> selectable) {
        ResetTilesColor();

        _tiles.Clear();
        selectable.ForEach(x => _tiles.Add(x.selectable as Tile));
        FilterTiles();
        
        ColorTiles();
    }

    private void OnDragStop() {
        StockpileCreator.CreateStockpileOnTiles(_tiles);
        ResetTilesColor();
        _tiles.Clear();
    }

    private void FilterTiles() {
        for (int i = _tiles.Count - 1; i >= 0; i--) {
            if (!Pathfinder.NodeAt(_tiles[i].position).isTraversable) {
                _tiles.RemoveAt(i);
            }
        }
    }

    private void ColorTiles() => _tiles.ForEach(x => x.SetColor(_stockpileColor));
    private void ResetTilesColor() => _tiles.ForEach(x => x.ResetColor());
}