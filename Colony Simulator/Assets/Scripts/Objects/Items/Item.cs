using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : IPlacable {
    
    public delegate void OnObjectDestroyed(Item item);
    public event OnObjectDestroyed OnDestroy;

    public abstract string Name { get; }

    public Vector2Int position { get; protected set; }

    public ItemScriptableObject data;
    public GameObject GameObject => _gameObject;

    protected GameObject _gameObject;
    protected abstract int StackCount { get; }

    public Item() { }

    public virtual void SetData(ItemScriptableObject data) => this.data = data;

    public virtual void SetGameObject(GameObject gameObject, Vector2Int position) {
        _gameObject = gameObject;
        _gameObject.transform.position = new Vector3(position.x, position.y, 0);
        this.position = position;
        PutOnTile();
    }

    public virtual void Destroy() {
        GameObject.Destroy(_gameObject);
        OnDestroy?.Invoke(this);
        RemoveFromTile();
    }

    #region IPlacable

    public void PutOnTile() => GameManager.Instance.world.GetTileAt(position).PutItemOnTile(this);
    public void RemoveFromTile() => GameManager.Instance.world.GetTileAt(position).RemoveItemFromTile();
    
    #endregion
}