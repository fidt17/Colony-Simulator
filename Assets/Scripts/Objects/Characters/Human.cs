using UnityEngine;

public class Human : Character {

    public MotionAnimatorComponent MotionAnimator      { get; private set; }
    public JobHandlerComponent     JobHandlerComponent { get; private set; }
    public InventoryComponent      InventoryComponent  { get; private set; }

    public override void SetGameObject(GameObject obj)  {
        base.SetGameObject(obj);
        InitializeMotionAnimator();
        InitializeJobHandler();
        InitializeInventory();
    }
    
    public override void Select() {
        base.Select();
        CommandInputStateMachine.SwitchCommandState(new MoveCommandInputState());
    }

    protected override void InitializeAI() {
        AI = new HumanAI(this);
        Components.Add(AI);
    }

    private void InitializeMotionAnimator() {
        MotionAnimator = new MotionAnimatorComponent(this);
        Components.Add(MotionAnimator);
    }

    private void InitializeJobHandler() {
        JobHandlerComponent = new JobHandlerComponent(this);
        Components.Add(JobHandlerComponent);
    }

    private void InitializeInventory() {
        InventoryComponent = new InventoryComponent(this);
        Components.Add(InventoryComponent);
    }
}