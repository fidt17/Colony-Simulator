using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandProcessor : MonoBehaviour
{   
    private Queue<Task> _taskList = new Queue<Task>();
    private Task _currentTask = null;

    public void AddTask(Task task) {

        _taskList.Enqueue(task);
    }

    public void AddUrgentTask(Task task) {

        ResetTasks();
        AddTask(task);
        StartExecution();
    }

    public void ResetTasks() {

        foreach(Task task in _taskList)
            task.FinishTask(false);

        _currentTask?.FinishTask(false);
        _currentTask = null;
        
        _taskList = new Queue<Task>();
    }

    public void StartExecution() {

        if (_currentTask == null)
            NextTask();
    }

    public void NextTask() {

        if (_taskList.Count == 0)
            return;

        _currentTask = _taskList.Dequeue();
        _currentTask.TaskResultHandler += OnTaskFinish;
    }

    private void Update() {

        _currentTask?.ExecuteTask();
    }

    private void OnTaskFinish(bool succeed) {

        _currentTask = null;
        NextTask();
    }

    public void OnDestroy() {

        if (_currentTask != null)
            _currentTask.TaskResultHandler -= OnTaskFinish;
    }
}
