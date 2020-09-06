﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WindowType {

    DebugWindow,
    DebugPathfinding
}

public class WindowComponent : MonoBehaviour
{
    public WindowType windowType;

    public List<WindowComponent> subWindows = new List<WindowComponent>();

    public void CloseWindow() {

        subWindows.ForEach(w => w.CloseWindow());
        subWindows = new List<WindowComponent>();
        gameObject.SetActive(false);
    }
}
