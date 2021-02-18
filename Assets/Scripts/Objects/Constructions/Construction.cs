using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Construction : StaticObject {

    public Construction() : base() {}

    #region IPrefab

    public void SetData(StaticScriptableObject data, Vector2Int position) {
        Data = data;
        this.Position = position;
        PutOnTile();
    } 

    public override void Destroy() {
        base.Destroy();
        RemoveFromTile();
    }

    #endregion
}