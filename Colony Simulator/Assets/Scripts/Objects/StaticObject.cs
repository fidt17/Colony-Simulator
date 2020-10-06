using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StaticObject : IGameObject {

    public delegate void OnObjectDestroyed(StaticObject staticObject);
    public event OnObjectDestroyed OnDestroy;

    public abstract string Name { get; }

    public StaticScriptableObject data;
    public GameObject GameObject => _gameObject;

    public Vector2Int position   { get; protected set; }
    public Vector2Int dimensions { get; protected set; }
    public bool isTraversable    { get; protected set; }

    protected GameObject _gameObject;
    
    public StaticObject() {}

    public StaticObject(Vector2Int dimensions) => this.dimensions = dimensions;

    public virtual void SetData(StaticScriptableObject data) => this.data = data;

    public virtual void SetGameObject(GameObject gameObject, Vector2Int position) {
        _gameObject = gameObject;
        _gameObject.transform.position = new Vector3(position.x, position.y, 0);
        this.position = position;
    }

    public virtual void Destroy() {
        GameObject.Destroy(_gameObject);
        OnDestroy?.Invoke(this);
    }
}
