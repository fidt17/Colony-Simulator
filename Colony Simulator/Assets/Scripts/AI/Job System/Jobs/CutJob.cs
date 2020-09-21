using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutJob : Job {
    
    protected IHarvestable _harvestable;

    public CutJob(IHarvestable harvestable, Vector2Int jobPosition) : base(jobPosition) {

        _harvestable = harvestable;
    }

    protected override void PlanJob() {

        _task = new Task();

        _task.AddCommand(new MoveCommand(_worker.MotionComponent, GetDestinationNode()));
        _task.AddCommand(new RotateToCommand(_worker.MotionComponent, GameManager.Instance.pathfinder.grid.GetNodeAt(_jobPosition)));
        _task.AddCommand(new WaitCommand(1f));
        _task.AddCommand(new HarvestCommand(_harvestable));

        _worker.CommandProcessor.AddTask(_task);

        _task.TaskResultHandler += OnJobFinish;
    }

    protected override void OnJobFinish(bool result) {

        if (result == true) {

            _worker.WithdrawJob();
        } else {

            _worker.WithdrawJob();
            JobSystem.Instance.AddJob(this);
        }
    }
}
