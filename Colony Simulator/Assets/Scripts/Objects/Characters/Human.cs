using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Character, IMotionAnimator {

    public override string Name => "human";

    #region Components

    public MotionAnimatorComponent motionAnimator { get; protected set; }

    #endregion

    public Human() { }

    public override void SetGameObject(GameObject gameObject, Vector2Int position) {

        base.SetGameObject(gameObject, position);
        InitializeMotionAnimator();
    }

    public override void Die() {

        base.Die();
        GameManager.Instance.characterManager.colonists.Remove(this);
    }

    #region Animation Component

    public void InitializeMotionAnimator() {

        motionAnimator = _gameObject.AddComponent<MotionAnimatorComponent>();
        motionAnimator.Initialize(motionComponent);
    }

    #endregion

    #region AI

    protected override void InitializeAI() {

        AI = _gameObject.AddComponent<HumanAI>();
        AI.Initialize(this);
    }

    #endregion
}