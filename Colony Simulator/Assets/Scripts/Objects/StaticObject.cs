using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StaticObject : IPrefab, IPlacable, IDestroyable {

    public event EventHandler OnDestroyed;

    public StaticScriptableObject data { get; protected set; }
    public GameObject gameObject       { get; protected set; }

    public Vector2Int position   { get; protected set; }
    public Vector2Int dimensions { get; protected set; }
    public bool isTraversable    { get; protected set; }

    #region IPrefab

    public virtual void SetData(PrefabScriptableObject data, Vector2Int position) {
        this.data = data as StaticScriptableObject;
        dimensions = this.data.dimensions;
        this.position = position;
        isTraversable = this.data.isTraversable;
        PutOnTile();
    }

    public virtual void SetGameObject(GameObject obj) {
        gameObject = obj;
        gameObject.transform.position = new Vector3(position.x, position.y, 0);
    }

    public virtual void Destroy() {
        GameObject.Destroy(gameObject);
        OnDestroyed?.Invoke(this, EventArgs.Empty);
        
        RemoveFromTile();
    }

    #endregion

    #region IPlacable

    public void PutOnTile() {
        Utils.TileAt(position).content.PutStaticObjectOnTile(this, isTraversable);
        AddToRegionContent();
    }

    public void RemoveFromTile() {
        Utils.TileAt(position).content.RemoveStaticObjectFromTile();
        RemoveFromRegionContent();
    }

    public virtual void AddToRegionContent() { } 
    public virtual void RemoveFromRegionContent() { }

    #endregion 
}
