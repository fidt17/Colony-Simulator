using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestCommand : Command {
    
    private IHarvestable _objectToHarvest;

    public HarvestCommand(IHarvestable objectToHarvest) {
        _objectToHarvest = objectToHarvest;
        (_objectToHarvest as IDestroyable).OnDestroyed += OnObjectDestroyedOutside;
    }

    public override void Execute() {
        (_objectToHarvest as IDestroyable).OnDestroyed -= OnObjectDestroyedOutside;
        _objectToHarvest.Harvest();
        Finish(true);
    }

    public override void Abort() {
        if (_objectToHarvest != null) {  
            (_objectToHarvest as IDestroyable).OnDestroyed -= OnObjectDestroyedOutside;
        }
    }
    
    public override void AbortDueToDestroy()
    {
        Abort();
    }

    protected void OnObjectDestroyedOutside(object source, EventArgs e) {
        (source as IDestroyable).OnDestroyed -= OnObjectDestroyedOutside;
        _objectToHarvest = null;
        Finish(false);
    }
}