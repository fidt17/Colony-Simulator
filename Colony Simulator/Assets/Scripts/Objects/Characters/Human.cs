using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Character, IMovable, IMotionAnimator
{   
    #region Components

    protected MotionComponent motionComponent;
    protected MotionAnimatorComponent motionAnimator;

    #endregion

    public Human(GameObject go) : base(go) {

        InitMotionComponent();
        InitMotionAnimator();
    }

    #region Motion Component

    public void InitMotionComponent() {

        motionComponent = this.gameObject.AddComponent<MotionComponent>();
        motionComponent.SetSpeed(5f);
        motionComponent.SetPosition(new Vector2(0, 0));
    }

    public void SetDestination(Tile destinationTile) {

        motionComponent.SetDestination(destinationTile);
    }

    public void SetPosition(Tile destinationTile) {

        motionComponent.SetPosition(destinationTile.position);
    }

    #endregion

    #region Animation Component

    public void InitMotionAnimator() {

        motionAnimator = this.gameObject.AddComponent<MotionAnimatorComponent>();
        motionComponent.VelocityHandler += new MotionComponent.OnVelocityChange(motionAnimator.HandleVelocity);
    }

    #endregion
}
