using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : IPrefab, IDestroyable, IPlacable {
    
    public event EventHandler OnDestroyed;

    public Vector2Int position { get; protected set; }

    public ItemScriptableObject data { get; protected set; }
    public GameObject gameObject     { get; protected set; }

    protected abstract int StackCount { get; }

    public void SetPosition(Vector2Int position) {
        this.position = position;
        gameObject.transform.position = Utils.ToVector3(this.position);
    }

    private void TryHaulingItemToAnyStockpile() {
        Pathfinder.UpdateHandler -= TryHaulingItemToAnyStockpile;
        if (Utils.TileAt(position).contents.stockpilePart != null) {
            return;
        }

        foreach (Job job in JobSystem.GetInstance().AllJobs) {
            if (job.GetType().Equals(typeof(HaulJob))) {
                HaulJob haulJob = job as HaulJob;
                if (haulJob.Item == this) {
                    return;
                }
            }
        }

        StockpilePart part = StockpileManager.FindStockpilePartForItem(this);
        if (part != null) {
            part.ChangeReservedState(true);
            JobSystem.GetInstance().AddJob(new HaulJob(this, Utils.NodeAt(part.position)));
        }    
    }
    
    #region IPrefab

    public virtual void SetData(PrefabScriptableObject data) => this.data = data as ItemScriptableObject;

    public virtual void SetGameObject(GameObject obj, Vector2Int position) {
        gameObject = obj;
        gameObject.transform.position = new Vector3(position.x, position.y, 0);
        this.position = position;
        PutOnTile();
        StockpileManager.OnNewStockpileCreated += TryHaulingItemToAnyStockpile;
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
        Utils.TileAt(position).contents.PutItemOnTile(this);
        Pathfinder.UpdateHandler += TryHaulingItemToAnyStockpile;
    }

    public void RemoveFromTile() => Utils.TileAt(position).contents.RemoveItemFromTile();
    
    #endregion
}