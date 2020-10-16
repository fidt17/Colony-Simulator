using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITask {
    event EventHandler TaskResultHandler;

    void AddCommand(Command command);
    void ExecuteTask();
    void NextCommand();
    void OnCommandFinish(object source, EventArgs e);
    void FinishTask();
    void AbortTask();
}