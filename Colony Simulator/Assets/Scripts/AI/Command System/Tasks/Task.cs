using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Task {

    public delegate void OnTaskResult(bool result);
    public event OnTaskResult TaskResultHandler;

    protected Queue<Command> _commandQueue = new Queue<Command>();
    protected Command _currentCommand;

    public void AddCommand(Command command) {
        command.CommandResultHandler += OnCommandFinish;
        _commandQueue.Enqueue(command);
    }

    public void ExecuteTask() {
        if (_currentCommand is null) {
            NextCommand();
        }
        _currentCommand?.Execute();
    }

    public void NextCommand() {
        if (_commandQueue.Count == 0) {
            FinishTask();
        } else {
            _currentCommand = _commandQueue.Dequeue();
        }
    }

    public void OnCommandFinish(bool succeed) {
        if (succeed) {
            NextCommand();
        } else {
            AbortTask();
        }
    }

    public void FinishTask() => TaskResultHandler?.Invoke(true);

    public void AbortTask() {
        foreach(Command command in _commandQueue) {
            command.Abort();
        }
        _currentCommand?.Abort();
        TaskResultHandler?.Invoke(false);
    }
}