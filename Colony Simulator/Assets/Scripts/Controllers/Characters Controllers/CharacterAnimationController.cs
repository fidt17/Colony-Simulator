using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAnimationController : MonoBehaviour
{
    public Animator animator;

    protected int velocityX, velocityY;

    public void SetAnimator() {

        animator = gameObject.GetComponent<Animator>();
    }
    public abstract void SetVelocity(Vector2 velocity);
}
