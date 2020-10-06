using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Task {

    public delegate void OnTaskResult(bool result);
    public event OnTaskResult TaskResultHandler;

    protected Queue<Command> _commandList = new Queue<Command>();
    protected Command _currentCommand;

    public void AddCommand(Command command) {
        command.CommandResultHandler += OnCommandFinish;
        _commandList.Enqueue(command);
    }

    public void ExecuteTask() {
        if (_currentCommand is null) {
            NextCommand();
        }
        _currentCommand?.Execute();
    }

    public void NextCommand() {
        if (_commandList.Count == 0) {
            FinishTask(true);
        } else {
            _currentCommand = _commandList.Dequeue();
        }
    }

    public void OnCommandFinish(bool succeed) {
        if (succeed) {
            NextCommand();
        } else {
            FinishTask(false);
        }
    }

    public void FinishTask(bool succeed) {
        if(!succeed) {
            foreach(Command command in _commandList) {
                command.Abort();
            }
            _currentCommand?.Abort();
        }
        TaskResultHandler?.Invoke(succeed);
    }
}