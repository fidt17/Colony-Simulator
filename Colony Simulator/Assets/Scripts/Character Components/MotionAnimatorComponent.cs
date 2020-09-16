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

    private void HandleVelocity(Vector2 velocity, FacingDirection direction) {

        _animator.SetFloat("velocityX", velocity.x);
        _animator.SetFloat("velocityY", velocity.y);

        _animator.SetInteger("directionIndex", (velocity != Vector2Int.zero) ? -1 : (int) direction);
    }

    private void OnDestroy() {

        motionComponent.VelocityHandler -= HandleVelocity;
    }
}
