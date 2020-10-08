using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobHandlerComponent : MonoBehaviour {

    public bool IsAvailable => _currentJob is null;

    public MotionComponent    MotionComponent  => _worker.motionComponent;
    public CommandProcessor   CommandProcessor => _worker.AI.CommandProcessor;
    public InventoryComponent Inventory        => _worker.inventoryComponent;

    private Human _worker;
    private Job _currentJob;

    public void Initialize(Human worker) {
        _worker = worker;
        JobSystem.GetInstance().AddWorker(this);
    }
    public void AssignJob(Job job) {
        _currentJob = job;
        _currentJob.AssignWorker(this);
        JobSystem.GetInstance().RemoveWorker(this);
    }

    public void WithdrawJob() {
        _currentJob = null;
        JobSystem.GetInstance().AddWorker(this);
    }

    public bool CanDoJob(Job job) {
        if (_currentJob != null) {
            return false;
        }

        PathNode jobNode = Pathfinder.NodeAt(job.Position);
        PathNode workerNode = Pathfinder.NodeAt(_worker.motionComponent.GridPosition);
        if (Pathfinder.FindNodeNear(jobNode, workerNode) is null) {
            return false;
        }

        return true;
    }
}