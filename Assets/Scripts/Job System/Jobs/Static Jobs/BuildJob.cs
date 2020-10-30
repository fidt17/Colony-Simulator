using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildJob : StaticJob {
    
    private ConstructionPlan _plan;

    public BuildJob(ConstructionPlan plan) : base(plan.Position) {
        _plan = plan;
    }

    protected override void PlanJob() {
        _task = new BuildTask(_plan, _worker.MotionComponent, GetDestinationNode().position) as ITask;
        _task.TaskResultHandler += OnJobFinish;
        _worker.CommandProcessor.AddTask(_task);
    }
}