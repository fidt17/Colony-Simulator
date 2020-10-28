using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulToItemHolderJob : HaulJob {
    
    public Type ItemType => _itemType;

    protected Vector2Int _dropPosition;
    protected IItemHolder _itemHolder;
    protected PathNode _destinationNode => Utils.NodeAt(_dropPosition);
    protected Item _item;
    protected Type _itemType;

    public HaulToItemHolderJob(Type itemType, IItemHolder itemHolder) : base((itemHolder as StaticObject).position) {
        _itemType = itemType;
        _dropPosition = (itemHolder as StaticObject).position;
        _itemHolder = itemHolder;
    }

    public override bool CanDoJob(JobHandlerComponent worker) {
        //check that worker can actually get to the dropPosition
        if (!base.CanDoJob(worker)) {
            return false;
        }

        //item search
        Func<Tile, bool> requirementsFunction = delegate(Tile tile) {
            if (tile == null) {
                return false;
            } else {
                if (tile.content.HasItem) {
                    return tile.content.item.GetType().Equals(ItemType) && tile.content.item.HasHaulJob == false;
                } else {
                    return false;
                }
            }
        };

        Tile t = SearchEngine.FindClosestBySubregionTileWhere(worker.MotionComponent.GridPosition, requirementsFunction, true);
        if (t == null) {
            return false;
        } else {
            _item = t.content.item;
            return true;
        }
        /////////////////
    }

    protected override void PlanJob() {
        _task = new HaulToItemHolderTask(_item, _itemHolder, _worker.MotionComponent, _worker.Inventory);

        _worker.CommandProcessor.AddTask(_task);
        _task.TaskResultHandler += OnJobFinish;
        _item.SetHaulJob(this);
    }
}