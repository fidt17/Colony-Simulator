using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Construction : StaticObject {

    public new ConstructionScriptableObject data { get; protected set; }
    
    public Construction() : base() {}

    #region IPrefab

    public override void SetData(PrefabScriptableObject data, Vector2Int position) {
        this.data = data as ConstructionScriptableObject;
        this.position = position;
        PutOnTile();
    } 

    public override void Destroy() {
        base.Destroy();
        RemoveFromTile();
    }

    #endregion
}