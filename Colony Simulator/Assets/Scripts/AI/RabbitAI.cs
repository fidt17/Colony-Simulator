using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAI : AIController {

    public Rabbit _rabbit;

    private IdleAIComponent _idleAIComponent;
    private HungerAIComponent _hungerAIComponent;

    public override void Initialize(Character character) {
        _rabbit = character as Rabbit;
        base.Initialize(character);
    }

    protected override void InitializeComponents() { 
        InitializeIdleAIComponent();
        InitializeHungerAIComponent();
    }

    protected override void DisableComponents() => DisableHungerAIComponent();

    #region Idle Component

    protected void InitializeIdleAIComponent() => _idleAIComponent = new IdleAIComponent(_rabbit);

    #endregion

    #region Hunger Component

    protected void InitializeHungerAIComponent() => _hungerAIComponent = new HungerAIComponent(_rabbit);
    protected void DisableHungerAIComponent() => _hungerAIComponent.UnassignListeners();

    #endregion
}