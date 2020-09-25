using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Job {
    
    public Vector2Int Position => _jobPosition;

    protected Vector2Int _jobPosition;
    protected JobHandlerComponent _worker;
    protected Task _task;

    protected GameObject _jobIcon;

    public Job() {}

    public Job(Vector2Int jobPosition) {
        _jobPosition = jobPosition;
        AddJobIcon();
    }

    public void AssignWorker(JobHandlerComponent worker) {
        _worker = worker;
        _worker.AssignJob(this);
        PlanJob();
    }

    public void WithdrawWorker() {
        if (_worker == null)
            return;

        if (_task != null) {
            _task.TaskResultHandler -= OnJobFinish;
            _worker.CommandProcessor.ResetTask(_task);
            _task = null;
        }

        _worker.WithdrawJob();
        _worker = null;
        JobSystem.Instance.ReturnJob(this);
    }

    protected abstract void PlanJob();
    protected abstract void AddJobIcon();

    protected void DeleteJobIcon() {
        if (_jobIcon != null) {
            GameObject.Destroy(_jobIcon);
        }
    }

    protected PathNode GetDestinationNode() {
        PathNode jobNode = GameManager.Instance.pathfinder.grid.GetNodeAt(_jobPosition);
        PathNode workerNode = GameManager.Instance.pathfinder.grid.GetNodeAt(_worker.MotionComponent.GridPosition);
        return GameManager.Instance.pathfinder.FindNodeNear(jobNode, workerNode);
    }

    protected void OnJobFinish(bool result) {
        if (result == true) {
            _worker.WithdrawJob();
            JobSystem.Instance.DeleteJob(this);
            DeleteJobIcon();
        } else {
            _worker.WithdrawJob();
            JobSystem.Instance.ReturnJob(this);
        }
    }
}