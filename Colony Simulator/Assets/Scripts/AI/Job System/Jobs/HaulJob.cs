using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaulJob : Job {
    
    public Item Item => _item;

    private PathNode _destinationNode;
    private Item _item;

    public HaulJob(Item item, PathNode destinationNode) : base(item.position) {
        _item = item;
        _destinationNode = destinationNode;
    }

    protected override void PlanJob() {
        _task = new Task();
        _task.AddCommand(new MoveCommand(_worker.MotionComponent, GetDestinationNode()));
        _task.AddCommand(new PickCommand(_item, _worker.Inventory));
        _task.AddCommand(new MoveCommand(_worker.MotionComponent, Pathfinder.FindNodeNear(_destinationNode, GetDestinationNode())));
        _task.AddCommand(new DropCommand(_item, _worker.Inventory, _destinationNode.position));

        _worker.CommandProcessor.AddTask(_task);
        _task.TaskResultHandler += OnJobFinish;

        StockpilePart part = Utils.TileAt(_destinationNode.position).contents.stockpilePart;
        if (part != null) {
            JobResultHandler += part.HaulJobResultHandler;
        }
    }
}