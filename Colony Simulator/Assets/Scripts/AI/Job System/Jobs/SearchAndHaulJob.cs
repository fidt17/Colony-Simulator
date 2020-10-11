using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchAndHaulJob : HaulJob {
    
    public Type ItemType => _itemType;
    public void SetItem(Item item) => _item = item;

    private Vector2Int _dropPosition;
    private PathNode _destinationNode => Utils.NodeAt(_dropPosition);
    private Item _item;
    private Type _itemType;

    public SearchAndHaulJob(Type itemType, Vector2Int dropPosition) : base(dropPosition) {
        _itemType = itemType;
        _dropPosition = dropPosition;
    }

    public override bool CanDoJob(JobHandlerComponent worker) {
        //accessibility
        bool b = base.CanDoJob(worker);
        if (b == false) {
            return false;
        }
        //item search
        Func<Tile, bool> requirementsFunction = delegate(Tile tile) {
            if (tile == null) {
                return false;
            } else {
                if (tile.contents.HasItem) {
                    return tile.contents.item.GetType().Equals(ItemType) && tile.contents.item.HasHaulJob == false;
                } else {
                    return false;
                }
            }
        };

        Tile t = SearchEngine.FindClosestTileWhere(worker.MotionComponent.GridPosition, requirementsFunction, true);
        if (t == null) {
            Debug.Log("item was not found");
            return false;
        } else {
            Debug.Log("item was found");
            SetItem(t.contents.item);
            return true;
        }
        /////////////////
    }

    protected override void PlanJob() {
        _task = new HaulTask(_item, _dropPosition, _worker.MotionComponent, _worker.Inventory);

        _worker.CommandProcessor.AddTask(_task);
        _task.TaskResultHandler += OnJobFinish;

        _item.SetHaulJob(this);
    }
}