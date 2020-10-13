using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTask : Task {

    public CutTask(MotionComponent motionComponent, Vector2Int targetPosition, IHarvestable tree) {
        this.AddCommand(new MoveCommand(motionComponent, targetPosition));
        this.AddCommand(new RotateToCommand(motionComponent, Utils.NodeAt((tree as StaticObject).position)));
        this.AddCommand(new WaitCommand(1f));
        this.AddCommand(new HarvestCommand(tree));
    }
}