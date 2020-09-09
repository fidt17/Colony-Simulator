using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{   
    public delegate void OnCommandResult(bool result);
    public event OnCommandResult CommandResultHandler;

    public bool inProgress = false;

    public virtual void Execute() {

        inProgress = true;
    }

    public virtual void Finish() {

        inProgress = false;
        OnCommandResultChanged(true);
    }

    public virtual void Abort() {

        inProgress = false;
        OnCommandResultChanged(false);
    }

    protected void OnCommandResultChanged(bool result) {

        CommandResultHandler?.Invoke(result);
    }
}
