using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCommand : Command {
    
    private IDestroyable _objectToDestroy;

    public DestroyCommand(IDestroyable objectToDestroy) {
        _objectToDestroy = objectToDestroy;
        _objectToDestroy.OnDestroyed += OnObjectDestroyedOutside;
    }

    public override void Execute() {
        _objectToDestroy.OnDestroyed -= OnObjectDestroyedOutside;
        _objectToDestroy.Destroy();
        Finish(true);
    }

    public override void Abort() {
        if (_objectToDestroy != null) {
            _objectToDestroy.OnDestroyed -= OnObjectDestroyedOutside;
        }
    }

    private void OnObjectDestroyedOutside(object sender, EventArgs e) {
        (sender as IDestroyable).OnDestroyed -= OnObjectDestroyedOutside;
        _objectToDestroy = null;
        Finish(false);
    }
}