using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StaticObject : IPrefab, IDestroyable {

    public event EventHandler OnDestroyed;

    public StaticScriptableObject data;
    public GameObject GameObject => _gameObject;

    public Vector2Int position   { get; protected set; }
    public Vector2Int dimensions { get; protected set; }
    public bool isTraversable    { get; protected set; }

    protected GameObject _gameObject;
    
    public StaticObject() {}

    public StaticObject(Vector2Int dimensions) => this.dimensions = dimensions;

    #region IPrefab

    public virtual void SetData(PrefabScriptableObject data) => this.data = data as StaticScriptableObject;

    public virtual void SetGameObject(GameObject obj, Vector2Int position) {
        _gameObject = obj;
        _gameObject.transform.position = new Vector3(position.x, position.y, 0);
        this.position = position;
    }

    public virtual void Destroy() {
        GameObject.Destroy(_gameObject);
        OnDestroyed?.Invoke(this, EventArgs.Empty);
    }

    #endregion
}
