using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAI : AIController
{
    public Rabbit character;

    #region AI Components

    IdleAIComponent idleAIComponent;
    HungerAIComponent hungerAIComponent;

    #endregion

    public override void Initialize(Character character) {

        this.character = character as Rabbit;
        base.Initialize(character);
    }

    protected override void InitializeComponents() { 

        InitializeIdleAIComponent();
        InitializeHungerAIComponent();
    }

    protected override void DisableComponents() { 

        DisableHungerAIComponent();
    }

    #region Idle Component

    protected void InitializeIdleAIComponent() => idleAIComponent = new IdleAIComponent(character, this);

    #endregion

    #region Hunger Component

    protected void InitializeHungerAIComponent() => hungerAIComponent = new HungerAIComponent(character, this);
    protected void DisableHungerAIComponent() => hungerAIComponent.UnassignListeners();

    #endregion
}
