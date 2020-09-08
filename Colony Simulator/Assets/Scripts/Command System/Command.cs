using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{   
    public delegate void OnCommandResult(bool result);
    public event OnCommandResult CommandResultHandler;

    public bool inProgress = false;

    public abstract void Execute();
    public abstract void Finish();
    public abstract void Abort();

    protected void OnCommandResultChanged(bool result) {

        CommandResultHandler?.Invoke(result);
    }
}
