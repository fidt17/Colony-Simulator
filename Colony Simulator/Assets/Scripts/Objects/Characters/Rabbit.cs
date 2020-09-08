using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Character, IMovable, IMotionAnimator
{   
    public override string Name {
        get {
            return "rabbit";
        }
    }

    #region Components

    public MotionAnimatorComponent motionAnimator { get; protected set; }

    #endregion

    public Rabbit() { }

    public override void SetGameObject(GameObject gameObject, Vector2Int position) {

        base.SetGameObject(gameObject, position);
        InitializeMotionAnimator();
    }

    #region Animation Component

    public void InitializeMotionAnimator() {

        motionAnimator = _gameObject.AddComponent<MotionAnimatorComponent>();

        motionComponent.VelocityHandler += new MotionComponent.OnVelocityChange(motionAnimator.HandleVelocity);
    }

    #endregion
}
