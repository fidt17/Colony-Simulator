using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour {

    public static DebugManager Instance;
    
    public PathfinderRenderer pathfinderRenderer;

    private void Awake() {

        if (Instance != null) {

            Debug.LogError("Only one DebugManager can exist at a time.");
            Destroy(gameObject);
        }

        Instance = this;
        pathfinderRenderer = GetComponent<PathfinderRenderer>();
    }

    #region Pathfinding

    public void OnDrawPathToggleChanged(bool toggleValue) {

        if (toggleValue) {
            GameManager.Instance.pathfinder.PathHandler += pathfinderRenderer.DrawPath;
        } else {
            GameManager.Instance.pathfinder.PathHandler -= pathfinderRenderer.DrawPath;
        }
    }

    public void OnDrawRegionsToggleChanged(bool toggleValue) => pathfinderRenderer.drawRegions = toggleValue;

    #endregion
}