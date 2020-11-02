using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HaulToItemHolderTask : Task {

    public HaulToItemHolderTask(Item item, IItemHolder itemHolder, MotionComponent motionComponent, InventoryComponent inventory) {
        PathNode itemNode = Utils.NodeAt(item.Position);
        PathNode humanNode = Utils.NodeAt(motionComponent.GridPosition);
        PathNode destinationNode = Utils.NodeAt((itemHolder as StaticObject).Position);

        AddCommand(new MoveCommand(motionComponent, Pathfinder.FindNodeNear(itemNode, humanNode).position));
        AddCommand(new PickCommand(item, inventory));
        AddCommand(new MoveCommand(motionComponent, Pathfinder.FindNodeNear(destinationNode, Pathfinder.FindNodeNear(itemNode, humanNode)).position));
        AddCommand(new DropIntoCommand(item, inventory, itemHolder));
    }

    public override void AbortTask() {
        foreach(Command command in CommandQueue) {
            command.Abort();
        }
        CurrentCommand?.Abort();
        Finish(false);
    }
}