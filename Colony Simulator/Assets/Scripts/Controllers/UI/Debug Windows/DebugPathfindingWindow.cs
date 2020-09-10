using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPathfindingWindow : WindowComponent
{   
    private Toggle _drawPathToggle;
    private Toggle _drawRegionsToggle;

    private void Awake() {

        FindWindowObjects();
    }

    private void Start() {

        _drawPathToggle.onValueChanged.AddListener(delegate{ DebugManager.Instance.OnDrawPathToggleChanged(_drawPathToggle.isOn); });
        _drawRegionsToggle.onValueChanged.AddListener(delegate{ DebugManager.Instance.OnDrawRegionsToggleChanged(_drawRegionsToggle.isOn); });
    }

    private void FindWindowObjects() {

        _drawPathToggle = transform.Find("Draw Path Toggle").GetComponent<Toggle>();
        _drawRegionsToggle = transform.Find("Draw Regions Toggle").GetComponent<Toggle>();
    }
}
