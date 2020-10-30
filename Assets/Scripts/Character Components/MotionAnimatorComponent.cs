using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionAnimatorComponent : CharacterComponent {

    private Animator _animator;
    private Character _character;

    public MotionAnimatorComponent(Character character) {
        _character = character;
        _character.motionComponent.VelocityHandler += HandleVelocity;
        _animator = _character.gameObject.GetComponent<Animator>();
    }

    private void HandleVelocity(Vector2 velocity, FacingDirection direction) {
        _animator.SetFloat("velocityX", velocity.x);
        _animator.SetFloat("velocityY", velocity.y);
        _animator.SetInteger("directionIndex", (velocity != Vector2Int.zero) ? -1 : (int) direction);
    }

    public override void DisableComponent() {
        base.DisableComponent();
        _character.motionComponent.VelocityHandler -= HandleVelocity;
        _character = null;
    }

    #region Testing

    public override bool CheckInitialization() {
        if (_character is null) {
            return false;
        }

        if (_animator is null) {
            return false;
        }

        return true;
    }

    #endregion
}