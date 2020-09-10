using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionAnimatorComponent : MonoBehaviour {

    private Animator _animator;

    private MotionComponent motionComponent;

    private void Awake() {

        _animator = gameObject.GetComponent<Animator>();
    }

    public void Initialize(MotionComponent motionComponent) {

        this.motionComponent = motionComponent;
        motionComponent.VelocityHandler += HandleVelocity;
    }

    private void HandleVelocity(Vector2 velocity) {

        _animator.SetFloat("velocityX", velocity.x);
        _animator.SetFloat("velocityY", velocity.y);
    }

    private void OnDestroy() {

        motionComponent.VelocityHandler -= HandleVelocity;
    }
}
