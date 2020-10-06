﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Character, IMotionAnimator {

    #region Components

    public MotionAnimatorComponent motionAnimator  { get; private set; }
    public JobHandlerComponent jobHandlerComponent { get; private set; }

    #endregion

    public Human() { }

    public override void SetGameObject(GameObject gameObject, Vector2Int position) {
        base.SetGameObject(gameObject, position);
        InitializeMotionAnimator();
        InitializeJobHandler();
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

    #region Job Handler
    
    private void InitializeJobHandler() {
        jobHandlerComponent = _gameObject.AddComponent<JobHandlerComponent>();
        jobHandlerComponent.Initialize(this);
    }

    #endregion

    #region Selection Component

    public override void OnSelect() {
        base.OnSelect();
        CommandInputStateMachine.SwitchCommandState(new MoveCommandInputState());
    }

    public override void OnDeselect() => base.OnDeselect();

    #endregion

    #region AI

    protected override void InitializeAI() {
        AI = _gameObject.AddComponent<HumanAI>();
        AI.Initialize(this);
    }

    #endregion
}