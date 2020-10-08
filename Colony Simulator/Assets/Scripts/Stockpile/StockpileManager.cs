using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StockpileManager {

    public static int StockpilesCount => _stockpiles.Count;

    private static List<Stockpile> _stockpiles = new List<Stockpile>();

    public static void AddStockpile(Stockpile stockpile) => _stockpiles.Add(stockpile);
    public static void RemoveStockpile(Stockpile stockpile) => _stockpiles.Remove(stockpile);
}