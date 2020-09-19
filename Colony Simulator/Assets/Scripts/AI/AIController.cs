using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIController : MonoBehaviour {

    public CommandProcessor commandProcessor { get; protected set; }

    public virtual void Initialize(Character character) {

        InitializeCommandProcessor();
        InitializeComponents();
    }

    protected virtual void OnDestroy() => DisableComponents();

    protected virtual void InitializeCommandProcessor() => commandProcessor = gameObject.AddComponent<CommandProcessor>();
    protected abstract void InitializeComponents();
    protected abstract void DisableComponents();
}
