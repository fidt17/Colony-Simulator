using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class HaulToItemHolderJob : HaulJob {
    
    public Type ItemType => _itemType;

    protected Vector2Int _dropPosition;
    protected IItemHolder _itemHolder;
    protected Item _item;
    protected Type _itemType;

    public HaulToItemHolderJob(Type itemType, IItemHolder itemHolder) : base((itemHolder as StaticObject).Position) {
        _itemType = itemType;
        _dropPosition = (itemHolder as StaticObject).Position;
        _itemHolder = itemHolder;
    }

    public override bool CanDoJob(JobHandlerComponent worker) {
        if (_wasJobCanceled) {
            JobSystem.GetInstance().DeleteJob(this);
            return false;
        }

        //check that worker can actually get to the dropPosition
        if (!base.CanDoJob(worker)) {
            return false;
        }

        //item search
        Func<Tile, bool> requirementsFunction = delegate(Tile tile) {
            if (tile == null) {
                return false;
            } else {
                if (tile.Contents.HasItem) {
                    return tile.Contents.Item.GetType().Equals(ItemType) && tile.Contents.Item.HasHaulJob == false;
                } else {
                    return false;
                }
            }
        };

        Tile t = SearchEngine.FindClosestBySubregionTileWhere(worker.MotionComponent.GridPosition, requirementsFunction, true);
        if (t == null) {
            JobSystem.GetInstance().StartCoroutine(JobSystem.GetInstance().HideJobForSeconds(this, 5f));
            return false;
        } else {
            _item = t.Contents.Item;
            return true;
        }
        /////////////////
    }

    protected override void PlanJob() {
        _task = new HaulToItemHolderTask(_item, _itemHolder, _worker.MotionComponent, _worker.Inventory);

        _worker.CommandProcessor.AddTask(_task);
        _task.ResultHandler += OnJobFinish;
        _item.SetHaulJob(this);
    }
}