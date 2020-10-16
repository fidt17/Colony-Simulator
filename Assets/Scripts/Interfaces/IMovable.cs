using UnityEngine;

public interface IMovable {
    MotionComponent motionComponent { get; }
    void InitializeMotionComponent(Vector2Int position);
}
