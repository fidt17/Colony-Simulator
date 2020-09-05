using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionAnimatorComponent : MonoBehaviour {

    public Animator animator;

    private void Awake() {

        animator = gameObject.GetComponent<Animator>();
    }

    public void HandleVelocity(Vector2 velocity) {

        animator.SetFloat("velocityX", velocity.x);
        animator.SetFloat("velocityY", velocity.y);
    }
}
