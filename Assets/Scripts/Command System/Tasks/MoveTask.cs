using UnityEngine;

public class MoveTask : Task {
	public MoveTask(MotionComponent motionComponent, Vector2Int destination) {
		AddCommand(new MoveCommand(motionComponent, destination));
	}
}
