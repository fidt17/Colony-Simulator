using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobHandlerComponent : CharacterComponent {

    public bool IsAvailable => _currentJob is null && CommandProcessor.HasTask == false;

    public MotionComponent    MotionComponent  => _worker.MotionComponent;
    public CommandProcessor   CommandProcessor => _worker.AI.CommandProcessor;
    public InventoryComponent Inventory        => _worker.InventoryComponent;

    private Human _worker;
    private Job _currentJob;

    public JobHandlerComponent(Human worker) {
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

    public override void DisableComponent() {
        base.DisableComponent();
        _currentJob?.DeleteJob();

        _currentJob = null;
        _worker = null;
    }

    #region Testing

    public override bool CheckInitialization() {

        if (_worker is null) {
            return false;
        }

        if (JobSystem.GetInstance().HasWorker(this) == false) {
            return false;
        }

        return true;
    }

    #endregion
}