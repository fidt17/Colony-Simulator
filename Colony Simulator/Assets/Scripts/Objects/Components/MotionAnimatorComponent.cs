using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionAnimatorComponent : MonoBehaviour {

    private Animator _animator;
    private MotionComponent _motionComponent;

    public void Initialize(MotionComponent motionComponent) {
        _motionComponent = motionComponent;
        _motionComponent.VelocityHandler += HandleVelocity;
    }

    private void Awake() {
        _animator = gameObject.GetComponent<Animator>();
        if (_animator is null) {
            Debug.LogWarning("No Animator was found.", this);
        }
    }

    private void OnDestroy() => _motionComponent.VelocityHandler -= HandleVelocity;

    private void HandleVelocity(Vector2 velocity, FacingDirection direction) {
        _animator.SetFloat("velocityX", velocity.x);
        _animator.SetFloat("velocityY", velocity.y);
        _animator.SetInteger("directionIndex", (velocity != Vector2Int.zero) ? -1 : (int) direction);
    }
}