using System;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class Item : IPrefab, IDestroyable {
    
    public event EventHandler OnDestroyed;

    public bool                 HasHaulJob { get; private set; }
    public ItemScriptableObject Data       { get; private set; }
    public GameObject           gameObject { get; private set; }
    public Vector2Int           Position   { get; private set; }

    protected abstract int StackCount { get; }
    
    public virtual void SetData(PrefabScriptableObject data, Vector2Int position) {
        this.Data = data as ItemScriptableObject;
        this.Position = position;
    } 

    public virtual void SetGameObject(GameObject obj) {
        gameObject = obj;
        gameObject.transform.position = Utils.ToVector3(Position);
        PutOnTile();
    }
    
    public void SetPosition(Vector2Int position) {
        Position = position;
        gameObject.transform.position = Utils.ToVector3(this.Position);
    }
    
    public virtual void Destroy() {
        Object.Destroy(gameObject);
        OnDestroyed?.Invoke(this, EventArgs.Empty);
        RemoveFromTile();
    }

    public void SetHaulJob(HaulJob job) {
        HasHaulJob = true;
        job.JobResultHandler += HaulJobResultHandler;
    }


    public void PutOnTile() {
        
        bool RequirementsFunction(Tile t) => t != null && !t.Contents.HasItem && t.IsTraversable;
        var tile = SearchEngine.FindClosestTileWhere(Position, RequirementsFunction, Utils.TileAt(Position).IsTraversable);
        
        if (tile != null) {
            SetPosition(tile.position);
            tile.Contents.PutItemOnTile(this);
            AddToRegionContent();
            StockpileManager.GetInstance().AddItem(this);
        } else {
            Destroy();
        }
    }

    public void RemoveFromTile() {
        Utils.TileAt(Position).Contents.RemoveItemFromTile();
        RemoveFromRegionContent();
        StockpileManager.GetInstance().RemoveItem(this);
    }

    public abstract void AddToRegionContent();

    protected abstract void RemoveFromRegionContent();
    
    private void HaulJobResultHandler(object source, EventArgs e) {
        ((Job) source).JobResultHandler -= HaulJobResultHandler;
        HasHaulJob = false;
    }
}