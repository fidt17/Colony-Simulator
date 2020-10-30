using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Task : ITask {

    public event EventHandler TaskResultHandler;
    public class TaskResultEventArgs : EventArgs {
        public bool result { get; set; }
    }

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

    public void OnCommandFinish(object source, EventArgs e) {
        if ((e as Command.CommandResultEventArgs).result == true) {
            NextCommand();
        } else {
            AbortTask();
        }
    }

    public void FinishTask() {
        OnResultChanged(true);
    }

    public virtual void AbortTask() {
        foreach(Command command in _commandQueue) {
            command.Abort();
        }
        _currentCommand?.Abort();
        OnResultChanged(false);
    }

    protected void OnResultChanged(bool result) {
        TaskResultEventArgs e = new TaskResultEventArgs();
        e.result = result;

        if (result == false) {
            int a;
        }

        TaskResultHandler?.Invoke(this, e);
    }
}