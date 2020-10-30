using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterComponent {

    public bool IsDisabled { get; protected set; } = false;
    protected List<Coroutine> _coroutines = new List<Coroutine>();

    public virtual void DisableComponent() {
        IsDisabled = true;
        foreach (Coroutine coroutine in _coroutines) {
            GameManager.GetInstance().StopCoroutine(coroutine);
        }
    }
    
    public abstract bool CheckInitialization();
}