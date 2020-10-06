using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Job {
    
    public Vector2Int Position => _jobPosition;

    protected Vector2Int _jobPosition;
    protected JobHandlerComponent _worker;
    protected Task _task;

    protected GameObject _jobIcon;

    public Job(Vector2Int jobPosition) {
        _jobPosition = jobPosition;
        AddJobIcon();
    }

    public void AssignWorker(JobHandlerComponent worker) {
        _worker = worker;
        PlanJob();
    }

    public void WithdrawWorker() {
        if (_worker is null) {
            return;
        }

        if (_task != null) {
            _task.TaskResultHandler -= OnJobFinish;
            _worker.CommandProcessor.ResetTask(_task);
            _task = null;
        }

        _worker.WithdrawJob();
        _worker = null;
        JobSystem.GetInstance().ReturnJob(this);
    }

    protected abstract void PlanJob();
    protected abstract void AddJobIcon();

    protected void DeleteJobIcon() {
        if (_jobIcon != null) {
            GameObject.Destroy(_jobIcon);
        }
    }

    protected PathNode GetDestinationNode() {
        PathNode jobNode = Pathfinder.NodeAt(_jobPosition);
        PathNode workerNode = Pathfinder.NodeAt(_worker.MotionComponent.GridPosition);
        return Pathfinder.FindNodeNear(jobNode, workerNode);
    }

    protected void OnJobFinish(bool result) {
        _worker.WithdrawJob();
        if (result == true) {
            JobSystem.GetInstance().DeleteJob(this);
            DeleteJobIcon();
        } else {
            JobSystem.GetInstance().ReturnJob(this);
        }
    }
}