using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StockpileManager {

    public delegate void StockpileCreationHandler();
    public static event StockpileCreationHandler OnNewStockpileCreated;

    public static int StockpilesCount => _stockpiles.Count;

    private static List<Stockpile> _stockpiles = new List<Stockpile>();

    public static void AddStockpile(Stockpile stockpile) {
        _stockpiles.Add(stockpile);
        OnNewStockpileCreated?.Invoke();
    }
    public static void RemoveStockpile(Stockpile stockpile) => _stockpiles.Remove(stockpile);

    public static StockpilePart FindStockpilePartForItem(Item item) {
        foreach(Stockpile stockpile in _stockpiles) {
            StockpilePart part = stockpile.FindPlaceForItem(item);
            if (part != null) {
                return part;
            }
        }
        return null;
    }
}