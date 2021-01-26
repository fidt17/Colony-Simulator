using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public abstract class Job {
    
    public event EventHandler JobResultHandler;
    public class JobResultEventArgs : EventArgs {
        public bool result;
        public bool wasJobCanceled;
    }

    public Vector2Int Position => _jobPosition;
    public bool _wasJobCanceled = false;

    protected Vector2Int _jobPosition;
    protected JobHandlerComponent _worker;
    protected ITask _task;

    protected GameObject _jobIcon;

    public Job(Vector2Int jobPosition, GameObject jobIcon = null) {
        _jobPosition = jobPosition;
        _jobIcon = jobIcon;
        AddJobIcon();
    }

    public virtual bool CanDoJob(JobHandlerComponent worker) {
        if (worker.IsAvailable == false) {
            return false;
        }

        Node jobNode = Utils.NodeAt(Position);
        Node workerNode = Utils.NodeAt(worker.MotionComponent.GridPosition);
        if (SearchEngine.FindNodeNear(jobNode, workerNode) is null) {
            return false;
        }

        return true;
    }

    public void AssignWorker(JobHandlerComponent worker) {
        _worker = worker;
        PlanJob();
    }

    public void DeleteJob() {
        if (_wasJobCanceled) {
            return;
        }
        
        _wasJobCanceled = true;
        if (_worker != null) {
            _worker.WithdrawJob();
            _task.ResultHandler -= OnJobFinish;
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

    protected Node GetDestinationNode() {
        Node jobNode = Utils.NodeAt(_jobPosition);
        Node workerNode = Utils.NodeAt(_worker.MotionComponent.GridPosition);
        return SearchEngine.FindNodeNear(jobNode, workerNode);
    }

    protected virtual void OnJobFinish(object source, System.EventArgs e) {
        _task.ResultHandler -= OnJobFinish;
        bool result = (e as Task.TaskResultEventArgs).Result;
        _worker.WithdrawJob();
        if (result == true) {
            JobSystem.GetInstance().DeleteJob(this);
            DeleteJobIcon();
        } else {
            JobSystem.GetInstance().ReturnJob(this);
        }
        OnJobResultChanged(result);
    }

    protected void OnJobResultChanged(bool result) {
        JobResultEventArgs e = new JobResultEventArgs();
        e.result = result;
        e.wasJobCanceled = _wasJobCanceled;
        JobResultHandler?.Invoke(this, e);
    }
}