using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CommandProcessor : MonoBehaviour {
    
    public bool HasTask   => _currentTask != null;
    public int  TaskCount => _taskQueue.Count;
    
    private readonly Queue<ITask> _taskQueue = new Queue<ITask>();
    private          ITask        _currentTask;

    private void Update() {
        _currentTask?.ExecuteTask();   
    }

    public void AddTask(ITask task) {
        _taskQueue.Enqueue(task);
        if (_currentTask is null) {
            NextTask();
        }
    }

    public void AddUrgentTask(ITask task) {
        AbortTasks();
        AddTask(task);
        NextTask();
    }

    private void NextTask() {
        if (_taskQueue.Count == 0) {
            return;
        }
        _currentTask = _taskQueue.Dequeue();
        _currentTask.ResultHandler += OnTaskFinish;
    }

    private void AbortTasks() {
        foreach(var task in _taskQueue) {
            task.AbortTask();
        }

        if (_currentTask != null) {
            _currentTask.ResultHandler -= OnTaskFinish;
            _currentTask.AbortTask();
            _currentTask = null;
        }
        
        _taskQueue.Clear();
    }

    private void AbortTaskDueToDeath()
    {
        foreach(var task in _taskQueue) {
            task.AbortTaskDueToDeath();
        }

        if (_currentTask != null) {
            _currentTask.ResultHandler -= OnTaskFinish;
            _currentTask.AbortTaskDueToDeath();
            _currentTask = null;
        }
        
        _taskQueue.Clear();
    }

    private void OnTaskFinish(object source, System.EventArgs e) {
        ((ITask) source).ResultHandler -= OnTaskFinish;
        _currentTask = null;
        NextTask();
    }
    
    private void OnDestroy() {
        AbortTaskDueToDeath();
    }
}