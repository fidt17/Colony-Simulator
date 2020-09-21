using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CommandProcessor : MonoBehaviour {
    
    public bool IsFree => _taskList.Count == 0;

    private Queue<Task> _taskList = new Queue<Task>();
    private Task _currentTask;

    private void Update() => _currentTask?.ExecuteTask();

    private void OnDestroy() {

        if (_currentTask != null)
            _currentTask.TaskResultHandler -= OnTaskFinish;
    }

    public void AddTask(Task task) {

        _taskList.Enqueue(task);
        
        if (_currentTask == null)
            StartExecution();
    }

    public void AddUrgentTask(Task task) {

        ResetTasks();
        AddTask(task);
        StartExecution();
    }

    public void ResetTask(Task task) {

        task.FinishTask(false);
        _taskList = new Queue<Task>(_taskList.Where(x => x != task));
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

    private void OnTaskFinish(bool succeed) {

        _currentTask = null;
        NextTask();
    }
}