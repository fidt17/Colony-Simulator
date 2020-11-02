using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StaticJob : Job {
    
    public StaticJob(Vector2Int jobPosition, GameObject jobIcon = null) : base (jobPosition, jobIcon) {
        AddToTile();
    }

    public void DeleteJob() {
        RemoveFromTile();
        base.DeleteJob();
    }

    protected void AddToTile() => Utils.TileAt(Position).Contents.AddStaticJob(this);
    protected void RemoveFromTile() => Utils.TileAt(Position).Contents.RemoveStaticJob(this);
}