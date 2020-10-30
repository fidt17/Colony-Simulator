using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Character {

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

    protected override void InitializeAI() {
        AI = gameObject.AddComponent<HumanAI>();
        AI.Initialize(this);
    }

    protected void InitializeMotionAnimator() {
        motionAnimator = new MotionAnimatorComponent(this);
        _components.Add(motionAnimator);
    }

    protected void InitializeJobHandler() {
        jobHandlerComponent = new JobHandlerComponent(this);
        _components.Add(jobHandlerComponent);
    }

    protected void InitializeInventory() {
        inventoryComponent = new InventoryComponent(this);
        _components.Add(inventoryComponent);
    }

    //Interface:
    //ISelectable
    public override void OnSelect() {
        base.OnSelect();
        CommandInputStateMachine.SwitchCommandState(new MoveCommandInputState());
    }
}