using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StockpileCreator {

    public static void CreateStockpileOnTiles(List<Tile> tiles) {
        Stockpile stockpile = new Stockpile();
        foreach (Tile tile in tiles) {
            if (tile.content.stockpilePart is null) {
                tile.content.stockpilePart = new StockpilePart(tile.position, stockpile);
            } else {
                tile.content.stockpilePart.ChangeStockpiles(stockpile);
            }
        }
        StockpileManager.GetInstance().AddStockpile(stockpile);
    }

    public static void RemoveStockpileFromTiles(List<Tile> tiles) {
        foreach (Tile tile in tiles) {
            tile.content.stockpilePart?.DeleteStockpilePart();
        }
    }

    public static void RemoveStockpileFromTile(Tile t) => RemoveStockpileFromTiles(new List<Tile>{t});
}