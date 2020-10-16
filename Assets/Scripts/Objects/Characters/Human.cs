using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Character, IMotionAnimator {

    #region Components

    public MotionAnimatorComponent motionAnimator  { get; private set; }
    public JobHandlerComponent jobHandlerComponent { get; private set; }
    public InventoryComponent inventoryComponent   { get; private set; }

    #endregion

    public override void SetGameObject(GameObject gameObject) {
        base.SetGameObject(gameObject);
        InitializeMotionAnimator();
        InitializeJobHandler();
        InitializeInventory();
    }

    public override void Die() {
        base.Die();
        GameManager.GetInstance().characterManager.colonists.Remove(this);
    }

    #region Animation Component

    public void InitializeMotionAnimator() {
        motionAnimator = gameObject.AddComponent<MotionAnimatorComponent>();
        motionAnimator.Initialize(motionComponent);
    }

    #endregion

    #region Job Handler
    
    private void InitializeJobHandler() {
        jobHandlerComponent = gameObject.AddComponent<JobHandlerComponent>();
        jobHandlerComponent.Initialize(this);
    }

    #endregion

    #region Selection Component

    public override void OnSelect() {
        base.OnSelect();
        CommandInputStateMachine.SwitchCommandState(new MoveCommandInputState());
    }

    #endregion

    #region AI

    protected override void InitializeAI() {
        AI = gameObject.AddComponent<HumanAI>();
        AI.Initialize(this);
    }

    #endregion

    #region InventoryComponent

    private void InitializeInventory() {
        inventoryComponent = gameObject.AddComponent<InventoryComponent>();
        inventoryComponent.Initialize(this);
    }

    #endregion
}