using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpileManager : Singleton<StockpileManager> {

    public int StockpilesCount => _stockpiles.Count;

    private List<Stockpile> _stockpiles = new List<Stockpile>();
    public List<Item> stockpileItems { get; private set; } = new List<Item>();
    public List<Item> otherItems { get; private set; } = new List<Item>();

    private const float haulJobCooldown = 1f;

    private void Start() => StartCoroutine(TryHaulingItems());

    public void AddStockpile(Stockpile stockpile) => _stockpiles.Add(stockpile);
    public void RemoveStockpile(Stockpile stockpile) => _stockpiles.Remove(stockpile);

    public StockpilePart FindStockpilePartForItem(Item item) {
        foreach(Stockpile stockpile in _stockpiles) {
            StockpilePart part = stockpile.FindPlaceForItem(item);
            if (part != null) {
                return part;
            }
        }
        return null;
    }

    public void AddItem(Item item) {
        if (item.Tile.content.stockpilePart != null) {
            AddItemToStockpiles(item);
        } else {
            otherItems.Add(item);
        }
    }

    public void AddItemToStockpiles(Item item) {
        stockpileItems.Add(item);
        otherItems.Remove(item);
    } 

    public void RemoveItem(Item item) {
        stockpileItems.Remove(item);
        otherItems.Remove(item);
    }

    public void RemoveItemFromStockpiles(Item item) {
        stockpileItems.Remove(item);
        otherItems.Add(item);
    }

    private void TryHaulingItemToAnyStockpile(Item item) {
        if (Utils.TileAt(item.position).content.stockpilePart != null || item.HasHaulJob) {
            return;
        }

        StockpilePart part = FindStockpilePartForItem(item);
        if (part != null) {
            HaulJob job = new HaulJob(item, part.position);
            part.SetHaulJob(job);
            JobSystem.GetInstance().AddJob(job);
        }    
    }

    //I should think of a more optimized solution.
    private IEnumerator TryHaulingItems() {
        while (true) {
            yield return new WaitForSeconds(haulJobCooldown);
            if (StockpilesCount == 0) {
                continue;
            }
            for (int i = otherItems.Count - 1; i >= 0; i--) {
                TryHaulingItemToAnyStockpile(otherItems[i]);
            }
        }
    }
}