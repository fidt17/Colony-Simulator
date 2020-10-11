using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command {
       
    public event System.EventHandler CommandResultHandler;
    public class CommandResultEventArgs : System.EventArgs {
        public bool result;
    }

    public abstract void Execute();

    public virtual void Finish(bool result) {
        CommandResultEventArgs e = new CommandResultEventArgs();
        e.result = result;
        CommandResultHandler?.Invoke(this, e);
    } 

    public virtual void Abort() {}
}