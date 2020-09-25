using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Character, IMovable, IMotionAnimator {

    public override string Name => "rabbit";

    #region Components

    public MotionAnimatorComponent motionAnimator { get; protected set; }

    #endregion

    public Rabbit() { }

    public override void SetGameObject(GameObject gameObject, Vector2Int position) {
        base.SetGameObject(gameObject, position);
        InitializeMotionAnimator();
    }

    public override void Die() {
        base.Die();
        GameManager.Instance.characterManager.rabbits.Remove(this);
    }

    #region Animation Component

    public void InitializeMotionAnimator() {
        motionAnimator = _gameObject.AddComponent<MotionAnimatorComponent>();
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
        AI = _gameObject.AddComponent<RabbitAI>();
        AI.Initialize(this);
    }

    #endregion
}