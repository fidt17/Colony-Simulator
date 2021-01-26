using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Construction : StaticObject {

    public ConstructionScriptableObject data { get; protected set; }
    
    public Construction() : base() {}

    #region IPrefab

    public void SetData(ConstructionScriptableObject data, Vector2Int position) {
        this.data = data as ConstructionScriptableObject;
        this.Position = position;
        PutOnTile();
    } 

    public override void Destroy() {
        base.Destroy();
        RemoveFromTile();
    }

    #endregion
}