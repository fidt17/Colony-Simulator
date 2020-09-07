using UnityEngine;

public interface IMotionAnimator
{
    MotionAnimatorComponent motionAnimator { get; }
    void InitializeMotionAnimator();
}
