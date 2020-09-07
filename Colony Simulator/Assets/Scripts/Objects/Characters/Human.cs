using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Character, IMovable, IMotionAnimator
{   
    #region Components

    public MotionComponent motionComponent { get; protected set; }
    public MotionAnimatorComponent motionAnimator { get; protected set; }

    #endregion

    public override string Name {

        get {
            return "human";
        }
    }

    public Human() { }

    public override void SetGameObject(GameObject gameObject) {

        base.SetGameObject(gameObject);
        InitializeMotionComponent();
        InitializeMotionAnimator();
    }

    #region Motion Component

    public void InitializeMotionComponent() {

        motionComponent = gameObject.AddComponent<MotionComponent>();

        motionComponent.SetSpeed(CharacterSpawnFactory.GetScriptableObject(Name).movementSpeed);
    }

    #endregion

    #region Animation Component

    public void InitializeMotionAnimator() {

        motionAnimator = gameObject.AddComponent<MotionAnimatorComponent>();

        motionComponent.VelocityHandler += new MotionComponent.OnVelocityChange(motionAnimator.HandleVelocity);
    }

    #endregion
}
