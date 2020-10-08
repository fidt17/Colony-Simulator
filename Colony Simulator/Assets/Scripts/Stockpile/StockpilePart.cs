using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpilePart {

    public Stockpile parent;
    public Vector2Int position { get; private set; }

    public GameObject gameObject;
    public SpriteRenderer spriteRenderer;

    public StockpilePart(Vector2Int position, Stockpile parent) {
        this.position = position;
        this.parent = parent;
        gameObject = Factory.Create("stockpile part", this.position);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        parent.AddPart(this);
    }

    public void ChangeStockpiles(Stockpile newStockpile) {
        parent.RemovePart(this);
        newStockpile.AddPart(this);
        parent = newStockpile;
    }
}