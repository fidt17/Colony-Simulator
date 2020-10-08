using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StockpileCreator {

    public static void CreateStockpileOnTiles(List<Tile> tiles) {
        Stockpile stockpile = new Stockpile();

        GameObject stockpileObject = new GameObject();
        stockpileObject.name = "Stockpile #" + (StockpileManager.StockpilesCount - 1);
        stockpile.gameObject = stockpileObject;

        foreach(Tile tile in tiles) {
            if (tile.contents.stockpilePart is null) {
                tile.contents.stockpilePart = new StockpilePart(tile.position, stockpile);
            } else {
                tile.contents.stockpilePart.ChangeStockpiles(stockpile);
            }
        }
        StockpileManager.AddStockpile(stockpile);
    }
}