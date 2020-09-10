using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandProcessor : MonoBehaviour
{   
    public Queue<Task> taskList = new Queue<Task>();

    private Task currentTask = null;

    public void StartExecution() {

        if (currentTask == null)
            NextTask();
    }

    public void ResetTasks() {

        foreach(Task task in taskList)
            task.FinishTask(false);

        currentTask?.FinishTask(false);
        currentTask = null;
        
        taskList = new Queue<Task>();
    }

    public void AddTask(Task task) {

        taskList.Enqueue(task);
    }

    public void AddUrgentTask(Task task) {

        ResetTasks();
        AddTask(task);
        StartExecution();
    }

    public void NextTask() {

        if (taskList.Count == 0)
            return;

        currentTask = taskList.Dequeue();
        currentTask.TaskResultHandler += OnTaskFinish;
    }

    private void OnTaskFinish(bool succeed) {

        currentTask = null;
        NextTask();
    }

    private void Update() {

        currentTask?.ExecuteTask();
    }
}
