using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Job {
    
    public delegate void ResultHandler(bool result);
    public event ResultHandler JobResultHandler;

    public Vector2Int Position => _jobPosition;

    protected Vector2Int _jobPosition;
    protected JobHandlerComponent _worker;
    protected ITask _task;

    protected GameObject _jobIcon;

    public Job(Vector2Int jobPosition) {
        _jobPosition = jobPosition;
        AddJobIcon();
    }

    public void AssignWorker(JobHandlerComponent worker) {
        _worker = worker;
        PlanJob();
    }

    public void DeleteJob() {
        if (_worker != null) {
            _worker.WithdrawJob();
            _task.TaskResultHandler -= OnJobFinish;
            _task.AbortTask();
        }
        DeleteJobIcon();
        JobSystem.GetInstance().DeleteJob(this);
        OnJobResultChanged(false);
    }

    protected abstract void PlanJob();
    
    protected virtual void AddJobIcon() {}

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

    protected virtual void OnJobFinish(object source, System.EventArgs e) {
        _task.TaskResultHandler -= OnJobFinish;
        bool result = (e as Task.TaskResultEventArgs).result;
        _worker.WithdrawJob();
        if (result == true) {
            JobSystem.GetInstance().DeleteJob(this);
            DeleteJobIcon();
        } else {
            JobSystem.GetInstance().ReturnJob(this);
        }
        OnJobResultChanged(result);
    }

    protected void OnJobResultChanged(bool result) => JobResultHandler?.Invoke(result);
}