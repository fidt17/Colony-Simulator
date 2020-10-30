using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTask : Task {

    public CutTask(MotionComponent motionComponent, Vector2Int targetPosition, ICuttable cuttable) {
        this.AddCommand(new MoveCommand(motionComponent, targetPosition));
        this.AddCommand(new RotateToCommand(motionComponent, Utils.NodeAt((cuttable as StaticObject).Position)));
        this.AddCommand(new WaitCommand(1f));
        this.AddCommand(new CutCommand(cuttable));
    }
}