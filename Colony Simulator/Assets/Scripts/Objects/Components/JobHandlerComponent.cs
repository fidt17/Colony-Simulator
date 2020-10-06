using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobHandlerComponent : MonoBehaviour {

    public bool IsAvailable => _currentJob == null;
    public MotionComponent MotionComponent => _worker.motionComponent;
    public CommandProcessor CommandProcessor => _worker.AI.commandProcessor;

    private Human _worker;
    private Job _currentJob;

    public void Initialize(Human worker) => _worker = worker;

    public void AssignJob(Job job) => _currentJob = job;
    public void WithdrawJob() => _currentJob = null;

    public bool CanDoJob(Job job) {
        if (_currentJob != null) {
            return false;
        }

        PathNode jobNode = GameManager.Instance.pathfinder.grid.GetNodeAt(job.Position);
        PathNode workerNode = GameManager.Instance.pathfinder.grid.GetNodeAt(_worker.motionComponent.GridPosition);
        if (GameManager.Instance.pathfinder.FindNodeNear(jobNode, workerNode) is null) {
            return false;
        }

        return true;
    }
}