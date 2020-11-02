using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTask : Task {

    public CutTask(MotionComponent motionComponent, Vector2Int targetPosition, ICuttable cuttable) {
        AddCommand(new MoveCommand(motionComponent, targetPosition));
        AddCommand(new RotateToCommand(motionComponent, Utils.NodeAt((cuttable as StaticObject).Position)));
        AddCommand(new WaitCommand(1f));
        AddCommand(new CutCommand(cuttable));
    }
}