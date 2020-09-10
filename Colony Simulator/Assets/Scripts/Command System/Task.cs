using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Task
{
    public Queue<Command> commandList = new Queue<Command>();

    public delegate void OnTaskResult(bool result);
    public event OnTaskResult TaskResultHandler;

    private Command currentCommand = null;

    public void AddCommand(Command command) {

        command.CommandResultHandler += OnCommandFinish;
        commandList.Enqueue(command);
    }

    public void ExecuteTask() {

        if (currentCommand == null)
            NextCommand();
        
        currentCommand?.Execute();
    }

    private void NextCommand() {

        if (commandList.Count == 0) {
            FinishTask(true);
        } else {
            currentCommand = commandList.Dequeue();
        }
    }

    private void OnCommandFinish(bool succeed) {

        if (succeed)
            NextCommand();
        else
            FinishTask(false);
    }

    public void FinishTask(bool succeed) {

        if (!succeed)
            currentCommand?.Abort();

        TaskResultHandler?.Invoke(succeed);
    }
}
