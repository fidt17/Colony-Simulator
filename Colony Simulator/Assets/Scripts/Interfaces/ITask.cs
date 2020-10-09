using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITask {
    event EventHandler TaskResultHandler;

    void AddCommand(Command command);
    void ExecuteTask();
    void NextCommand();
    void OnCommandFinish(bool succeed);
    void FinishTask();
    void AbortTask();
}