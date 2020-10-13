using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulJob : Job {
    
    public Item Item => _item;

    private Vector2Int _destinationPosition;
    private PathNode _destinationNode => Utils.NodeAt(_destinationPosition);
    private Item _item;

    public HaulJob(Vector2Int destinationPosition) : base(destinationPosition) {}
    
    public HaulJob(Item item, Vector2Int destinationPosition) : base(item.position) {
        _item = item;
        _destinationPosition = destinationPosition;
    }

    protected override void PlanJob() {
        _task = new HaulTask(_item, _destinationPosition, _worker.MotionComponent, _worker.Inventory);

        _worker.CommandProcessor.AddTask(_task);
        _task.TaskResultHandler += OnJobFinish;

        _item.SetHaulJob(this);

        StockpilePart part = Utils.TileAt(_destinationNode.position).content.stockpilePart;
        if (part != null) {
            JobResultHandler += part.HaulJobResultHandler;
        }
    }

    protected override void OnJobFinish(object source, System.EventArgs e) {
        bool result = (e as Task.TaskResultEventArgs).result;
        if (result == true) {
            _worker.WithdrawJob();
            JobSystem.GetInstance().DeleteJob(this);
            DeleteJobIcon();
            OnJobResultChanged(result);
        } else {
            DeleteJob();
        }
    }
}