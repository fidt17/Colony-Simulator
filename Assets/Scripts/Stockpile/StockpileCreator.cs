using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StockpileCreator {

    public static void CreateStockpileOnTiles(List<Tile> tiles) {
        Stockpile stockpile = new Stockpile();
        foreach (Tile tile in tiles) {
            if (tile.Contents.StockpilePart is null) {
                tile.Contents.SetStockpilePart(new StockpilePart(tile.position, stockpile));
            } else {
                tile.Contents.StockpilePart.ChangeStockpiles(stockpile);
            }
        }
        StockpileManager.GetInstance().AddStockpile(stockpile);
    }

    public static void RemoveStockpileFromTiles(List<Tile> tiles) {
        foreach (Tile tile in tiles) {
            tile.Contents.StockpilePart?.DeleteStockpilePart();
        }
    }

    public static void RemoveStockpileFromTile(Tile t) => RemoveStockpileFromTiles(new List<Tile>{t});
}