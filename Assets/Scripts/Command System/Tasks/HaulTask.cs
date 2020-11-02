using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HaulTask : Task {

    public HaulTask(Item item, Vector2Int destinationPosition, MotionComponent motionComponent, InventoryComponent inventory) {
        PathNode itemNode = Utils.NodeAt(item.Position);
        PathNode humanNode = Utils.NodeAt(motionComponent.GridPosition);
        PathNode destinationNode = Utils.NodeAt(destinationPosition);

        AddCommand(new MoveCommand(motionComponent, Pathfinder.FindNodeNear(itemNode, humanNode).position));
        AddCommand(new PickCommand(item, inventory));
        AddCommand(new MoveCommand(motionComponent, Pathfinder.FindNodeNear(destinationNode, Pathfinder.FindNodeNear(itemNode, humanNode)).position));
        AddCommand(new DropCommand(item, inventory, destinationPosition));
    }

    public override void AbortTask() {
        foreach(Command command in CommandQueue) {
            command.Abort();
        }
        CurrentCommand?.Abort();
        Finish(false);
    }
}