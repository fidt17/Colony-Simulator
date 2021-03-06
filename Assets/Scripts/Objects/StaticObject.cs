﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Interfaces;
using Object = UnityEngine.Object;

public abstract class StaticObject : IPrefab, IDestroyable, IPosition {

    public event EventHandler OnDestroyed;

    public StaticScriptableObject Data       { get; protected set; }
    public GameObject             gameObject { get; protected set; }

    public Vector2Int Position   { get; protected set; }
    public bool IsTraversable    { get; protected set; }

    public virtual void SetData(PrefabScriptableObject data, Vector2Int position) {
        Data = data as StaticScriptableObject;
        IsTraversable = Data.isTraversable;
        Position = position;
        PutOnTile();
    }

    public virtual void SetGameObject(GameObject obj) {
        gameObject = obj;
        gameObject.transform.position = Utils.ToVector3(Position);
    }

    public virtual void Destroy() {
        Object.Destroy(gameObject);
        OnDestroyed?.Invoke(this, EventArgs.Empty);
        RemoveFromTile();
    }

    protected void PutOnTile() {
        Utils.TileAt(Position).Contents.PutStaticObjectOnTile(this, IsTraversable);
        AddToRegionContent();
    }

    protected void RemoveFromTile() {
        Utils.TileAt(Position).Contents.RemoveStaticObjectFromTile();
        RemoveFromRegionContent();
    }

    public    virtual void AddToRegionContent() { }
    protected virtual void RemoveFromRegionContent() { }
}
