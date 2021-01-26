using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class DebugManager : Singleton<DebugManager> {
    
    public PathfinderRenderer pathfinderRenderer;

    protected override void Awake() {
        pathfinderRenderer = GetComponent<PathfinderRenderer>();
    }

    #region Pathfinding

    public void OnDrawPathToggleChanged(bool toggleValue) {
        if (toggleValue) {
            Pathfinder.PathHandler += pathfinderRenderer.DrawPath;
        } else {
            Pathfinder.PathHandler -= pathfinderRenderer.DrawPath;
        }
    }

    public void OnDrawRegionsToggleChanged(bool toggleValue) => pathfinderRenderer.drawRegions = toggleValue;

    #endregion
}