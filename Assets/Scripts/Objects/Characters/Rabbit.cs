using UnityEngine;

public class Rabbit : Character {

    public MotionAnimatorComponent MotionAnimator { get; private set; }

    public override void SetGameObject(GameObject obj) {
        base.SetGameObject(obj);
        InitializeMotionAnimator();
    }

    private void InitializeMotionAnimator() {
        MotionAnimator = new MotionAnimatorComponent(this);
        Components.Add(MotionAnimator);
    }

    protected override void InitializeHungerComponent() {
        base.InitializeHungerComponent();
        HungerComponent.Edibles.Add(typeof(Grass));
    }

    protected override void InitializeAI() {
        AI = new RabbitAI(this);
        Components.Add(AI);
    }
}