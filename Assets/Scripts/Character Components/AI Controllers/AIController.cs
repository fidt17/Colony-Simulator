using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIController : CharacterComponent {

    public readonly CommandProcessor CommandProcessor;

    protected AIController(Character character) {
        CommandProcessor = character.gameObject.AddComponent<CommandProcessor>();
    }

    public abstract override void DisableComponent();
    public abstract override bool CheckInitialization();
}