using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Construction : StaticObject, IPlacable {

    public new ConstructionScriptableObject data { get; protected set; }
    
    public Construction() : base() {}

    #region IPrefab

    public override void SetData(PrefabScriptableObject data) {
        this.data = data as ConstructionScriptableObject;
    } 

    public override void SetGameObject(GameObject gameObject, Vector2Int position) {
        base.SetGameObject(gameObject, position);
        PutOnTile();
    }

    public override void Destroy() {
        base.Destroy();
        RemoveFromTile();
    }

    #endregion

    #region IPlacable

    public void PutOnTile()      => Utils.TileAt(position).contents.PutStaticObjectOnTile(this, isTraversable);
    public void RemoveFromTile() => Utils.TileAt(position).contents.RemoveStaticObjectFromTile();
    
    #endregion
}