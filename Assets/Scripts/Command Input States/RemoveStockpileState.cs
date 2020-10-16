using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveStockpileState : StockpileState {

    protected override void UpdateCursorTexture() => CursorManager.Instance.SwitchTexture(CursorManager.Instance.cutStateTexture);

    protected override void OnAreaChange(object source, EventArgs e) {
        SelectionTracker.OnAreaChangeArgs args = e as SelectionTracker.OnAreaChangeArgs;

        ResetTilesColor();
        _tiles = Utils.GetTilesInArea(args.start, args.end);
        FilterTiles();
        if (args.dragEnded) {
            StockpileCreator.RemoveStockpileFromTiles(_tiles);
            ResetTilesColor();
            _tiles.Clear();
        } else {
            ResetTilesColor();
            ColorTiles();
        }
    }
}