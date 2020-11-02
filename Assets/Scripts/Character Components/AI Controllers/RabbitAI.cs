using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAI : AIController {

    private readonly Rabbit _rabbit;

    private readonly IdleAIComponent _idleAIComponent;

    public RabbitAI(Rabbit rabbit) : base(rabbit) {
        _rabbit          = rabbit;
        _idleAIComponent = new IdleAIComponent(_rabbit, CommandProcessor);
    }
    
    public override void DisableComponent() {
        base.DisableComponent();
        _idleAIComponent.DisableComponent();
    }
    
    public override bool CheckInitialization() {

        if (_rabbit is null) {
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