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
    private bool isRunning = false;

    private bool is_disposed = false;

    public void AddCommand(Command command) {

        commandList.Enqueue(command);
    }

    public void ExecuteTask() {

        if (isRunning == false) {

            NextCommand();
        } else {

            if (currentCommand == null)
                return;

            if (currentCommand.inProgress == false)
                currentCommand.CommandResultHandler += FinishCommand;

            currentCommand.Execute();
        }
    }

    private void NextCommand() {

        currentCommand = null;

        if (commandList.Count == 0) {

            FinishTask(true);
            return;
        }

        isRunning = true;
        currentCommand = commandList.Dequeue();
    }

    private void FinishCommand(bool succeed) {

        currentCommand.CommandResultHandler -= FinishCommand;

        if (succeed)
            NextCommand();
        else
            FinishTask(false);
    }

    public void FinishTask(bool succeed) {

        TaskResultHandler?.Invoke(succeed);
    }
}
