using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAI : AIController {

    public Rabbit _rabbit;

    private IdleAIComponent _idleAIComponent;

    public override void Initialize(Character character) {
        _rabbit = character as Rabbit;
        base.Initialize(character);
    }

    protected override void InitializeComponents() { 
        InitializeIdleAIComponent();
    }
    protected override void DisableComponents() { }

    #region Idle Component

    protected void InitializeIdleAIComponent() => _idleAIComponent = new IdleAIComponent(_rabbit);

    #endregion
}