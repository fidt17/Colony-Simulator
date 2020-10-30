using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpilePart {

    public Stockpile parent;
    public Vector2Int position { get; private set; }

    public GameObject gameObject;
    public SpriteRenderer spriteRenderer;

    public HaulJob haulJob;
    public bool HasItem => GetItem != null;
    public Item GetItem => Utils.TileAt(position).content.Item;

    public StockpilePart(Vector2Int position, Stockpile parent) {
        this.position = position;
        this.parent = parent;
        gameObject = Factory.Create("stockpile part", this.position);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        parent.AddPart(this);

        if (HasItem) {
            StockpileManager.GetInstance().AddItemToStockpiles(GetItem);
        }
    }

    public void ChangeStockpiles(Stockpile newStockpile) {
        parent.RemovePart(this);
        newStockpile.AddPart(this);
        parent = newStockpile;
    }

    public void SetHaulJob(HaulJob haulJob) => this.haulJob = haulJob;

    public void DeleteStockpilePart() {
        //destroy gameObject
        GameObject.Destroy(gameObject);
        //remove from stockpile
        if (HasItem) {
            StockpileManager.GetInstance().RemoveItemFromStockpiles(GetItem);
        }
        parent.RemovePart(this);
        Utils.TileAt(position).content.RemoveStockpilePart();
        //reset existing hauling jobs to this stockpile part
        TryDeletingHaulingJob();
    }

    public void TryDeletingHaulingJob() {
        if (haulJob != null) {
            haulJob.JobResultHandler -= HaulJobResultHandler;
            haulJob.DeleteJob();
            haulJob = null;
        }
    }

    public void HaulJobResultHandler(object source, EventArgs e) {
        haulJob.JobResultHandler -= HaulJobResultHandler;
        haulJob = null;
    }
}