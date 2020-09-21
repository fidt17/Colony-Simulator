﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command {
       
    public delegate void OnCommandResult(bool result);
    public event OnCommandResult CommandResultHandler;

    public abstract void Execute();

    public virtual void Finish(bool result) => OnCommandResultChanged(result);
    public virtual void Abort() {}

    protected void OnCommandResultChanged(bool result) => CommandResultHandler?.Invoke(result);
}