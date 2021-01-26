using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;

public class HaulTask : Task {

    public HaulTask(Item item, Vector2Int destinationPosition, MotionComponent motionComponent, InventoryComponent inventory) {
        Node itemNode = Utils.NodeAt(item.Position);
        Node humanNode = Utils.NodeAt(motionComponent.GridPosition);
        Node destinationNode = Utils.NodeAt(destinationPosition);

        AddCommand(new MoveCommand(motionComponent, SearchEngine.FindNodeNear(itemNode, humanNode).Position));
        AddCommand(new PickCommand(item, inventory));
        AddCommand(new MoveCommand(motionComponent, SearchEngine.FindNodeNear(destinationNode, SearchEngine.FindNodeNear(itemNode, humanNode)).Position));
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