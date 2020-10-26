using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTask : Task {

    public BuildTask(ConstructionPlan plan, MotionComponent motionComponent, Vector2Int targetPosition) {
        this.AddCommand(new MoveCommand(motionComponent, targetPosition));
        this.AddCommand(new RotateToCommand(motionComponent, Utils.NodeAt(plan.position)));
        this.AddCommand(new WaitCommand(1f));
        this.AddCommand(new BuildCommand(plan));
    }
}