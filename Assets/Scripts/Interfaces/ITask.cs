using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITask {
    event EventHandler ResultHandler;
    void ExecuteTask();
    void AbortTask();
}