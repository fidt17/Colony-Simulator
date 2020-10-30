using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CommandProcessor : MonoBehaviour {
    
    public bool HasTask => _currentTask != null;

    private Queue<ITask> _taskQueue = new Queue<ITask>();
    private ITask _currentTask;

    private void Update() => _currentTask?.ExecuteTask();

    private void OnDestroy() {
        if (_currentTask != null) {
            _currentTask.TaskResultHandler -= OnTaskFinish;
        }
    }

    public void AddTask(ITask task) {
        _taskQueue.Enqueue(task);
        StartExecution();
    }

    public void AddUrgentTask(ITask task) {
        ResetTasks();
        AddTask(task);
        StartExecution();
    }

    public void ResetTask(ITask task) {
        task.AbortTask();
        _taskQueue = new Queue<ITask>(_taskQueue.Where(x => x != task));
    }

    public void ResetTasks() {
        foreach(ITask task in _taskQueue) {
            task.AbortTask();
        }
        _currentTask?.AbortTask();
        _currentTask = null;
        _taskQueue.Clear();
    }

    public void StartExecution() {
        if (_currentTask is null) {
            NextTask();
        }
    }

    public void NextTask() {
        if (_taskQueue.Count == 0) {
            return;
        }
        _currentTask = _taskQueue.Dequeue();
        _currentTask.TaskResultHandler += OnTaskFinish;
    }

    private void OnTaskFinish(object source, System.EventArgs e) {
        if (_currentTask != null) {
            (source as ITask).TaskResultHandler -= OnTaskFinish;
            _currentTask = null;
        }
        NextTask();
    }
}