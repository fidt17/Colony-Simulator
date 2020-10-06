using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Character, IMovable, IMotionAnimator {

    #region Components

    public MotionAnimatorComponent motionAnimator { get; protected set; }

    #endregion

    public override void SetGameObject(GameObject gameObject, Vector2Int position) {
        base.SetGameObject(gameObject, position);
        InitializeMotionAnimator();
    }

    public override void Die() {
        base.Die();
        GameManager.GetInstance().characterManager.rabbits.Remove(this);
    }

    #region Animation Component

    public void InitializeMotionAnimator() {
        motionAnimator = gameObject.AddComponent<MotionAnimatorComponent>();
        motionAnimator.Initialize(motionComponent);
    }

    #endregion

    #region Hunger Component

    public override void InitializeHungerComponent() {
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