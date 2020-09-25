using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAI : AIController {

    public Human character;

    #region AI Components

    private IdleAIComponent _idleAIComponent;

    #endregion

    public override void Initialize(Character character) {
        this.character = character as Human;
        base.Initialize(character);
    }

    protected override void InitializeComponents() => InitializeIdleAIComponent();

    protected override void DisableComponents() { }

    #region Idle Component

    protected void InitializeIdleAIComponent() => _idleAIComponent = new IdleAIComponent(character);

    #endregion
}
