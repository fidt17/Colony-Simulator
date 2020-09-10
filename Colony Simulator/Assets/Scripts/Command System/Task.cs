using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Task
{
    private Queue<Command> _commandList = new Queue<Command>();
    private Command _currentCommand = null;

    public delegate void OnTaskResult(bool result);
    public event OnTaskResult TaskResultHandler;

    public void AddCommand(Command command) {

        command.CommandResultHandler += OnCommandFinish;
        _commandList.Enqueue(command);
    }

    public void ExecuteTask() {

        if (_currentCommand == null)
            NextCommand();
        
        _currentCommand?.Execute();
    }

    private void NextCommand() {

        if (_commandList.Count == 0) {
            FinishTask(true);
        } else {
            _currentCommand = _commandList.Dequeue();
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
            _currentCommand?.Abort();

        TaskResultHandler?.Invoke(succeed);
    }
}
