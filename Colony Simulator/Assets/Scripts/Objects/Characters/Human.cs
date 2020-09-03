﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Character
{
    public Human(GameObject go) : base(go) {

        motionController.SetSpeed(5f);
        SetAnimationController();
    }

    protected override void SetAnimationController() {

        animationController = gameObject.AddComponent<HumanAnimationController>();
        animationController.SetAnimator();
    }
}
