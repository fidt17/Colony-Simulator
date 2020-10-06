using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAI : AIController {

    private Human _human;

    private IdleAIComponent _idleAIComponent;

    public override void Initialize(Character character) {
        _human = character as Human;
        base.Initialize(character);
    }

    protected override void InitializeComponents() => InitializeIdleAIComponent();
    protected override void DisableComponents() { }

    #region Idle Component

    protected void InitializeIdleAIComponent() => _idleAIComponent = new IdleAIComponent(_human);

    #endregion
}
