using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAnimationController : CharacterAnimationController
{
    public override void SetVelocity(Vector2 velocity) {

        animator.SetFloat("velocityX", velocity.x);
        animator.SetFloat("velocityY", velocity.y);
    }
}
