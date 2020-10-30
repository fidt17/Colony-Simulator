using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StockpileCreator {

    public static void CreateStockpileOnTiles(List<Tile> tiles) {
        Stockpile stockpile = new Stockpile();
        foreach (Tile tile in tiles) {
            if (tile.content.StockpilePart is null) {
                tile.content.SetStockpilePart(new StockpilePart(tile.position, stockpile));
            } else {
                tile.content.StockpilePart.ChangeStockpiles(stockpile);
            }
        }
        StockpileManager.GetInstance().AddStockpile(stockpile);
    }

    public static void RemoveStockpileFromTiles(List<Tile> tiles) {
        foreach (Tile tile in tiles) {
            tile.content.StockpilePart?.DeleteStockpilePart();
        }
    }

    public static void RemoveStockpileFromTile(Tile t) => RemoveStockpileFromTiles(new List<Tile>{t});
}