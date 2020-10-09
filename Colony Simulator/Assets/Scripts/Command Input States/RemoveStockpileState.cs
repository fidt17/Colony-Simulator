using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveStockpileState : StockpileState {

    protected override void UpdateCursorTexture() => CursorManager.Instance.SwitchTexture(CursorManager.Instance.cutStateTexture);

    protected override void OnDragUpdate(List<SelectableComponent> selectable) {
        ResetTilesColor();

        _tiles.Clear();
        selectable.ForEach(x => _tiles.Add(x.selectable as Tile));
        FilterTiles();
        
        ColorTiles();
    }

    protected override void OnDragStop() {
        StockpileCreator.RemoveStockpileFromTiles(_tiles);
        ResetTilesColor();
        _tiles.Clear();
    }
}