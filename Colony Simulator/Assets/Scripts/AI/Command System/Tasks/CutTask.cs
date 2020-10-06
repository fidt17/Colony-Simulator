using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTask : Task {

    public CutTask(Character character, PathNode targetNode, Tree tree) {
        this.AddCommand(new MoveCommand(character.motionComponent, targetNode));
        this.AddCommand(new RotateToCommand(character.motionComponent, Pathfinder.NodeAt(tree.position)));
        this.AddCommand(new WaitCommand(1f));
        this.AddCommand(new HarvestCommand(tree));
    }
}