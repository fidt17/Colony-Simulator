using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : IPrefab, IPlacable {
    
    public delegate void OnObjectDestroyed(Item item);
    public event OnObjectDestroyed OnDestroy;

    public Vector2Int position { get; protected set; }

    public ItemScriptableObject data;
    public GameObject GameObject => _gameObject;

    protected GameObject _gameObject;
    protected abstract int StackCount { get; }

    public Item() { }

    #region IPrefab

    public virtual void SetData(PrefabScriptableObject data) => this.data = data as ItemScriptableObject;

    public virtual void SetGameObject(GameObject obj, Vector2Int position) {
        _gameObject = obj;
        _gameObject.transform.position = new Vector3(position.x, position.y, 0);
        this.position = position;
        PutOnTile();
    }

    public virtual void Destroy() {
        GameObject.Destroy(_gameObject);
        OnDestroy?.Invoke(this);
        RemoveFromTile();
    }

    #endregion

    #region IPlacable

    public void PutOnTile() => GameManager.Instance.world.GetTileAt(position).PutItemOnTile(this);
    public void RemoveFromTile() => GameManager.Instance.world.GetTileAt(position).RemoveItemFromTile();
    
    #endregion
}