using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileContents {

    public bool HasItem => item != null;

    public StockpilePart stockpilePart;
    public StaticObject staticObject;
    public Item item;

    private Tile _tile;

    public TileContents(Tile tile) => _tile = tile;

    public void PutStaticObjectOnTile(StaticObject staticObject, bool isTraversable) {
        if (this.staticObject != null) {
            RemoveStaticObjectFromTile();
        }
        this.staticObject = staticObject;
        _tile.SetTraversability(isTraversable);
    }

    public void RemoveStaticObjectFromTile() {
        staticObject = null;
        _tile.SetTraversability(true);
    }

    public void PutItemOnTile(Item item) {
        if (this.item != null) {
            Debug.LogError("Item was destroyed because another item was placed upon it." + " pos: " + this.item.position);
            this.item.Destroy();
        }
        this.item = item;
    }

    public void RemoveItemFromTile() => item = null;
}