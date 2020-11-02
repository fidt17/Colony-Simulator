using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAI : AIController {

    private Human _human;

    private readonly IdleAIComponent _idleAIComponent;

    public HumanAI(Human human) : base(human) {
        _human = human;
        _idleAIComponent = new IdleAIComponent(_human, CommandProcessor);
    }

    public override void DisableComponent() {
        base.DisableComponent();
        _human = null;
        _idleAIComponent.DisableComponent();
    }

    public override bool CheckInitialization() {

        if (_human is null) {
            return false;
        }

        if (CommandProcessor is null) {
            return false;
        }

        if (_idleAIComponent is null) {
            return false;
        }

        return true;
    }
}
