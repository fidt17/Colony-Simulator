using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutCommand : Command {
    
    private ICuttable _cuttable;

    public CutCommand(ICuttable cuttable) {
        _cuttable = cuttable;
        (_cuttable as IDestroyable).OnDestroyed += OnObjectDestroyedOutside;
    }

    public override void Execute() {
        (_cuttable as IDestroyable).OnDestroyed -= OnObjectDestroyedOutside;
        _cuttable.Cut();
        Finish(true);
    }

    public override void Abort() {
        if (_cuttable != null) {  
            (_cuttable as IDestroyable).OnDestroyed -= OnObjectDestroyedOutside;
        }
    }

    public override void AbortDueToDestroy()
    {
        Abort();
    }

    protected void OnObjectDestroyedOutside(object source, EventArgs e) {
        (source as IDestroyable).OnDestroyed -= OnObjectDestroyedOutside;
        _cuttable = null;
        Finish(false);
    }
}