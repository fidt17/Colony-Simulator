using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAI : AIController {

    public Rabbit character;

    #region AI Components

    private IdleAIComponent _idleAIComponent;
    private HungerAIComponent _hungerAIComponent;

    #endregion

    public override void Initialize(Character character) {
        this.character = character as Rabbit;
        base.Initialize(character);
    }

    protected override void InitializeComponents() { 
        InitializeIdleAIComponent();
        InitializeHungerAIComponent();
    }

    protected override void DisableComponents() => DisableHungerAIComponent();

    #region Idle Component

    protected void InitializeIdleAIComponent() => _idleAIComponent = new IdleAIComponent(character);

    #endregion

    #region Hunger Component

    protected void InitializeHungerAIComponent() => _hungerAIComponent = new HungerAIComponent(character);
    protected void DisableHungerAIComponent() => _hungerAIComponent.UnassignListeners();

    #endregion
}