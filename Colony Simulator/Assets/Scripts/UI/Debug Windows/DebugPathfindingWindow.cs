﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPathfindingWindow : WindowComponent
{   
    [SerializeField] private Toggle _drawPathToggle = null;
    [SerializeField] private Toggle _drawRegionsToggle = null;

    private void Start() {

        _drawPathToggle.onValueChanged.AddListener(delegate{ DebugManager.Instance.OnDrawPathToggleChanged(_drawPathToggle.isOn); });
        _drawRegionsToggle.onValueChanged.AddListener(delegate{ DebugManager.Instance.OnDrawRegionsToggleChanged(_drawRegionsToggle.isOn); });
    }
}
