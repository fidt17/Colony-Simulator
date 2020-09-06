using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPathfindingWindow : WindowComponent
{   
    public Toggle drawPathToggle;
    public Toggle drawRegionsToggle;

    private void Awake() {

        FindWindowObjects();
    }

    private void Start() {

        drawPathToggle.onValueChanged.AddListener(delegate{ DebugManager.Instance.OnDrawPathToggleChanged(drawPathToggle.isOn); });
        drawRegionsToggle.onValueChanged.AddListener(delegate{ DebugManager.Instance.OnDrawRegionsToggleChanged(drawRegionsToggle.isOn); });
    }

    private void FindWindowObjects() {

        drawPathToggle = transform.Find("Draw Path Toggle").GetComponent<Toggle>();
        drawRegionsToggle = transform.Find("Draw Regions Toggle").GetComponent<Toggle>();
    }
}
