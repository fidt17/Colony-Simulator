using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Character {

    #region Components

    public MotionAnimatorComponent motionAnimator { get; protected set; }

    #endregion

    public override void SetGameObject(GameObject gameObject) {
        base.SetGameObject(gameObject);
        InitializeMotionAnimator();
    }

    #region Animation Component

    protected void InitializeMotionAnimator() {
        motionAnimator = new MotionAnimatorComponent(this);
        _components.Add(motionAnimator);
    }

    #endregion

    #region Hunger Component

    protected override void InitializeHungerComponent() {
        base.InitializeHungerComponent();
        hungerComponent.edibles.Add(typeof(Grass));
    }

    #endregion

    #region AI

    protected override void InitializeAI() {
        AI = gameObject.AddComponent<RabbitAI>();
        AI.Initialize(this);
    }

    #endregion
}