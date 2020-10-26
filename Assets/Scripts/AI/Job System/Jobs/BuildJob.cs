using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildJob : Job {
    
    private ConstructionPlan _plan;

    public BuildJob(ConstructionPlan plan) : base(plan.position) {
        _plan = plan;
    }

    protected override void PlanJob() {
        _task = new BuildTask(_plan, _worker.MotionComponent, GetDestinationNode().position) as ITask;
        _worker.CommandProcessor.AddTask(_task);
        _task.TaskResultHandler += OnJobFinish;
    }
}