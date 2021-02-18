using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterComponent {

    public bool IsDisabled { get; private set; }
    
    private readonly List<Coroutine> _coroutines = new List<Coroutine>();
    
    public virtual void DisableComponent() {
        IsDisabled = true;
        var gameManager = GameManager.GetInstance();
        foreach (var coroutine in _coroutines) {
            if (coroutine != null)
            {
                gameManager.StopCoroutine(coroutine);
            }
        }
        _coroutines.Clear();
    }
    
    public abstract bool CheckInitialization();

    protected void RunCoroutine(IEnumerator coroutine) {
        _coroutines.Add(GameManager.GetInstance().StartCoroutine(coroutine));
    }
}