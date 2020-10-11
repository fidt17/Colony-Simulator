using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : IPrefab, IDestroyable, IPlacable {
    
    public event EventHandler OnDestroyed;

    public Vector2Int position { get; protected set; }

    public ItemScriptableObject data { get; protected set; }
    public GameObject gameObject     { get; protected set; }

    public Tile Tile => Utils.TileAt(position);
    public bool HasHaulJob => _haulJob != null;

    private HaulJob _haulJob;

    protected abstract int StackCount { get; }

    public void SetPosition(Vector2Int position) {
        this.position = position;
        gameObject.transform.position = Utils.ToVector3(this.position);
    }

    public void SetHaulJob(HaulJob job) {
        _haulJob = job;
        job.JobResultHandler += HaulJobResultHandler;
    }

    public void HaulJobResultHandler(object source, EventArgs e) {
        (source as Job).JobResultHandler -= HaulJobResultHandler;
        _haulJob = null;
    }

    #region IPrefab

    public virtual void SetData(PrefabScriptableObject data) => this.data = data as ItemScriptableObject;

    public virtual void SetGameObject(GameObject obj, Vector2Int position) {
        gameObject = obj;
        gameObject.transform.position = new Vector3(position.x, position.y, 0);
        this.position = position;
        PutOnTile();
    }

    #endregion

    #region IDestroyable
    
    public virtual void Destroy() {
        GameObject.Destroy(gameObject);
        OnDestroyed?.Invoke(this, EventArgs.Empty);
        RemoveFromTile();
    }

    #endregion

    #region IPlacable

    public void PutOnTile() {

        Func<Tile, bool> requirementsFunction = delegate(Tile t) {
            if (t == null) {
                return false;
            } else {
                return !t.contents.HasItem;
            }
        };
        Tile tile = SearchEngine.FindClosestTileWhere(position, requirementsFunction);
        
        if (tile != null) {
            SetPosition(tile.position);
            tile.contents.PutItemOnTile(this);
            StockpileManager.GetInstance().AddItem(this);
        } else {
            Destroy();
        }
    }

    public void RemoveFromTile() {
        Tile.contents.RemoveItemFromTile();
        StockpileManager.GetInstance().RemoveItem(this);
    }
    
    #endregion
}