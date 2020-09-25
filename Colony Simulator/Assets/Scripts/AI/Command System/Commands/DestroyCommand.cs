using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : Command {
    
    private StaticObject _objectToDestroy;

    public Destroy(StaticObject objectToDestroy) {
        _objectToDestroy = objectToDestroy;
        _objectToDestroy.OnDestroy += OnObjectDestroyedOutside;
    }

    public override void Execute() {
        _objectToDestroy.OnDestroy -= OnObjectDestroyedOutside;
        _objectToDestroy.Destroy();
        Finish(true);
    }

    //if objects gets destroyed before this command can destroy the object itself.
    protected void OnObjectDestroyedOutside(StaticObject s) {
        _objectToDestroy = null;
        Finish(false);
    }

    public override void Abort() {
        if (_objectToDestroy != null) {
            _objectToDestroy.OnDestroy -= OnObjectDestroyedOutside;
        }
    }
}