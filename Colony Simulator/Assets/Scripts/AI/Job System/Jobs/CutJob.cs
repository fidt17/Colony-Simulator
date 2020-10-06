using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutJob : Job {
    
    public IHarvestable Vegetation => _harvestable;

    protected IHarvestable _harvestable;

    public CutJob(IHarvestable harvestable, Vector2Int jobPosition) : base(jobPosition) => _harvestable = harvestable;

    protected override void AddJobIcon() => _jobIcon = Factory.Create("cut job", _jobPosition);

    protected override void PlanJob() {
        _task = new Task();
        _task.AddCommand(new MoveCommand(_worker.MotionComponent, GetDestinationNode()));
        _task.AddCommand(new RotateToCommand(_worker.MotionComponent, GameManager.Instance.pathfinder.grid.GetNodeAt(_jobPosition)));
        _task.AddCommand(new WaitCommand(1f));
        _task.AddCommand(new HarvestCommand(_harvestable));

        _worker.CommandProcessor.AddTask(_task);
        _task.TaskResultHandler += OnJobFinish;
    }
}