using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCommand : Command {
    
    private ConstructionPlan _plan;

    public BuildCommand(ConstructionPlan plan) {
        _plan = plan;
    }

    public override void Execute() {
        _plan.Build();
        Finish(true);
    }

    public override void Abort()
    { }

    public override void AbortDueToDestroy()
    { }
}