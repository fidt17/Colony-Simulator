﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Character, IMotionAnimator
{   
    public override string Name {

        get {
            return "human";
        }
    }

    #region Components

    public MotionAnimatorComponent motionAnimator { get; protected set; }

    #endregion

    public Human() { }

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
