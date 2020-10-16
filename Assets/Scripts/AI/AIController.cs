using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIController : MonoBehaviour {

    public CommandProcessor CommandProcessor => _commandProcessor;

    private CommandProcessor _commandProcessor;

    public virtual void Initialize(Character character) {
        InitializeCommandProcessor();
        InitializeComponents();
    }

    protected virtual void OnDestroy() => DisableComponents();
    protected virtual void InitializeCommandProcessor() => _commandProcessor = gameObject.AddComponent<CommandProcessor>();

    protected abstract void InitializeComponents();
    protected abstract void DisableComponents();
}