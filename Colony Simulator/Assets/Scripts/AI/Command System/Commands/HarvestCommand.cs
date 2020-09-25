using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestCommand : Command {
    
    private IHarvestable _objectToHarvest;

    public HarvestCommand(IHarvestable objectToHarvest) {
        _objectToHarvest = objectToHarvest;
        ((StaticObject) _objectToHarvest).OnDestroy += OnObjectDestroyedOutside;
    }

    public override void Execute() {
        ((StaticObject) _objectToHarvest).OnDestroy -= OnObjectDestroyedOutside;
        _objectToHarvest.Harvest();
        Finish(true);
    }

    public override void Abort() {
        if (_objectToHarvest != null) {  
            ((StaticObject) _objectToHarvest).OnDestroy -= OnObjectDestroyedOutside;
        }
    }

    protected void OnObjectDestroyedOutside(StaticObject s) {
        _objectToHarvest = null;
        Finish(false);
    }
}