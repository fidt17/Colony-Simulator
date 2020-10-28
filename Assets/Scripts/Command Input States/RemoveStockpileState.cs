using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveStockpileState : StockpileState {

    protected override string ColorHex => "#A64141";

    protected override void UpdateCursorTexture() => CursorManager.Instance.SwitchTexture(CursorManager.Instance.cutStateTexture);

    protected override void OnAreaChange(object source, EventArgs e) {
        if (source is SelectionTracker) {
            SelectionTracker.OnAreaChangeArgs args = e as SelectionTracker.OnAreaChangeArgs;

            if (_selectionArea?.CompareTo(args.rectangle) == false || _selectionArea == null) {
                _selectionArea = args.rectangle;
                DrawSelectionArea(args.startMousePosition, args.currentMousePosition);
            }

            if (args.dragEnded) {
                StockpileCreator.RemoveStockpileFromTiles(GetTilesInArea());
                GameObject.Destroy(_area?.gameObject);
            }
        }
    }
}