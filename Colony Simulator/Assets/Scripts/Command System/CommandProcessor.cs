using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandProcessor : MonoBehaviour
{   
    public Queue<Task> taskList = new Queue<Task>();

    private Task currentTask = null;
    public bool isRunning = false;

    public void AddTask(Task task) {

        taskList.Enqueue(task);
    }

    public void ResetTasks() {

        foreach(Task task in taskList)
            task.FinishTask(false);
        
        taskList = new Queue<Task>();
        isRunning = false;
    }

    public void NextTask() {

        currentTask = null;

        if (taskList.Count == 0) {
            isRunning = false;
            return;
        }

        isRunning = true;
        currentTask = taskList.Dequeue();
        currentTask.TaskResultHandler += FinishTask;
    }

    private void ExecuteCurrentTask() {

        if (isRunning == false)
            NextTask();
        else
            currentTask.ExecuteTask();
    }

    private void FinishTask(bool succeed) {

        if (succeed)
            NextTask();
        else
            ResetTasks();
    }

    private void Update() {

        if (isRunning)
            ExecuteCurrentTask();
    }
}
