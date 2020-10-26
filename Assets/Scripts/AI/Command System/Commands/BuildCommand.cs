using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCommand : Command {
    
    private ConstructionPlan _plan;

    public BuildCommand(ConstructionPlan plan) {
        _plan = plan;
        //FIX ME : CREATE JOB CANCLE EVENT OR SMTH
    }

    public override void Execute() {
        //FIX ME : UNSUBSCRIBE FROM EVENT
        _plan.Build();
        Finish(true);
    }

    public override void Abort() {
        if (_plan != null) {
            //FIX ME : UNSUBSCRIBE FROM EVENT
        }
    }
}