using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Character, IMovable, IMotionAnimator
{   
    public override string Name {
        get {
            return "rabbit";
        }
    }

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

        motionComponent.VelocityHandler -= motionAnimator.HandleVelocity;
    }

    #region Animation Component

    public void InitializeMotionAnimator() {

        motionAnimator = _gameObject.AddComponent<MotionAnimatorComponent>();

        motionComponent.VelocityHandler += motionAnimator.HandleVelocity;
    }

    #endregion

    #region Hunger Component

    public override void InitializeHungerComponent() {

        base.InitializeHungerComponent();
        hungerComponent.hungerTick = 0.5f;
        hungerComponent.edibles.Add(typeof(Grass));
    }

    #endregion
}
