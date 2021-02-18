using System.Collections.Generic;
using System;

public class Task : ITask {

    public event EventHandler ResultHandler;
    public class TaskResultEventArgs : EventArgs {
        public bool Result;
        public bool Death;
    }

    public int  CommandsCount => CommandQueue.Count;
    public Type CurrentCommandType   => CurrentCommand.GetType();
    
    protected readonly Queue<Command> CommandQueue = new Queue<Command>();
    protected Command CurrentCommand;

    public void AddCommand(Command command) {
        CommandQueue.Enqueue(command);
        command.ResultHandler += OnCommandFinish;
    }

    public void ExecuteTask() {
        if (CurrentCommand is null) {
            NextCommand();
        }
        CurrentCommand?.Execute();
    }

    public virtual void AbortTask() {
        foreach(Command command in CommandQueue) {
            command.Abort();
        }

        if (CurrentCommand != null) {
            CurrentCommand.ResultHandler -= OnCommandFinish;
            CurrentCommand.Abort();
        }
        
        Finish(false);
    }

    public void AbortTaskDueToDeath()
    {
        foreach(Command command in CommandQueue) {
            command.AbortDueToDestroy();
        }

        if (CurrentCommand != null) {
            CurrentCommand.ResultHandler -= OnCommandFinish;
            CurrentCommand.AbortDueToDestroy();
        }
        
        Finish(false, true);
    }

    public System.Delegate[] GetResultHandlerSubscribers() {
        return ResultHandler?.GetInvocationList();
    }

    private void NextCommand() {
        if (CommandQueue.Count == 0) {
            Finish(true);
        } else {
            CurrentCommand = CommandQueue.Dequeue();
        }
    }

    private void OnCommandFinish(object source, EventArgs e) {
        if (((Command.CommandResultEventArgs) e).Result == true) {
            ((Command) source).ResultHandler -= OnCommandFinish;
            NextCommand();
        } else {
            AbortTask();
        }
    }

    protected void Finish(bool result, bool death = false) {
        var e = new TaskResultEventArgs {
            Result = result,
            Death = death
        };
        ResultHandler?.Invoke(this, e);
    }
}