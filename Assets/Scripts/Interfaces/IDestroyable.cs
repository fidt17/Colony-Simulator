using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IDestroyable {
    event EventHandler OnDestroyed;
    void Destroy();
}