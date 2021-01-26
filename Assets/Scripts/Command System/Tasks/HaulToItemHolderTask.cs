using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;

public class HaulToItemHolderTask : Task {

    public HaulToItemHolderTask(Item item, IItemHolder itemHolder, MotionComponent motionComponent, InventoryComponent inventory) {
        Node itemNode = Utils.NodeAt(item.Position);
        Node humanNode = Utils.NodeAt(motionComponent.GridPosition);
        Node destinationNode = Utils.NodeAt((itemHolder as StaticObject).Position);

        AddCommand(new MoveCommand(motionComponent, SearchEngine.FindNodeNear(itemNode, humanNode).Position));
        AddCommand(new PickCommand(item, inventory));
        AddCommand(new MoveCommand(motionComponent, SearchEngine.FindNodeNear(destinationNode, SearchEngine.FindNodeNear(itemNode, humanNode)).Position));
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