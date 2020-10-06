using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command {
       
    public delegate void OnCommandResult(bool result);
    public event OnCommandResult CommandResultHandler;

    public abstract void Execute();

    public virtual void Finish(bool result) => CommandResultHandler?.Invoke(result);
    public virtual void Abort() {}
}