using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command {
       
    public event System.EventHandler ResultHandler;
    public class CommandResultEventArgs : System.EventArgs {
        public bool Result;
    }

    public abstract void Execute();

    protected virtual void Finish(bool result) {
        var e = new CommandResultEventArgs {
            Result = result
        };
        ResultHandler?.Invoke(this, e);
    }

    public abstract void Abort();
    public abstract void AbortDueToDestroy();

    public System.Delegate[] GetResultHandlerSubscribers() {
        return ResultHandler?.GetInvocationList();
    }    
}