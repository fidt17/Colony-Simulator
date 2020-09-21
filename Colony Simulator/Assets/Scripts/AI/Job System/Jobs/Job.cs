using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Job {
    
    public Vector2Int Position => _jobPosition;

    protected Vector2Int _jobPosition;
    protected JobHandlerComponent _worker;
    protected Task _task;

    public Job(Vector2Int jobPosition) {

        _jobPosition = jobPosition;
    }

    public void AssignWorker(JobHandlerComponent worker) {

        _worker = worker;
        _worker.AssignJob(this);
        PlanJob();
    }

    protected abstract void PlanJob();

    public void WithdrowWorker() {

        if (_worker == null)
            return;

        if (_task != null) {

            _task.TaskResultHandler -= OnJobFinish;
            _worker.CommandProcessor.ResetTask(_task);
            _task = null;
        }

        _worker.WithdrawJob();
        _worker = null;
        JobSystem.Instance.AddJob(this);
    }

    public void DeleteJob() {

        if (_worker != null)
            WithdrowWorker();

        JobSystem.Instance.DeleteJob(this);
    }

    protected abstract void OnJobFinish(bool result);

    protected virtual PathNode GetDestinationNode() {

        PathNode jobNode = GameManager.Instance.pathfinder.grid.GetNodeAt(_jobPosition);
        PathNode workerNode = GameManager.Instance.pathfinder.grid.GetNodeAt(_worker.MotionComponent.GetGridPosition());

        return GameManager.Instance.pathfinder.FindNodeNear(jobNode, workerNode);
    }
}
