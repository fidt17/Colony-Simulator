using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutJob : StaticJob {
    
    public IHarvestable Vegetation => _harvestable;

    protected IHarvestable _harvestable;
    
    public CutJob(IHarvestable harvestable, Vector2Int jobPosition, GameObject jobIcon = null) : base(jobPosition, jobIcon) {
        _harvestable = harvestable;
    }

    protected override void AddJobIcon() => _jobIcon = (_jobIcon is null) ? Factory.Create("cut job", _jobPosition) : _jobIcon;

    protected override void PlanJob() {
        _task = new CutTask(_worker.MotionComponent, GetDestinationNode().position, _harvestable) as ITask;
        _worker.CommandProcessor.AddTask(_task);
        _task.TaskResultHandler += OnJobFinish;
    }
}