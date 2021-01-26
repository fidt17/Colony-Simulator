using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Tile : IData, ITraversable {
    
    public event EventHandler OnTraversabilityChange;
    
    public TileType type { get; set; }
    public TileContent Contents { get; private set; }

    public TileScriptableObject data { get; protected set; }
    public Sprite GetSprite() => data.prefabSprite;
    public Color GetColor() => data.defaultColor;

    public Vector2Int position   { get; protected set; }
    public bool IsTraversable    { get; protected set; }

    public void SetData(PrefabScriptableObject data, Vector2Int position) {
        this.data = data as TileScriptableObject;
        IsTraversable = this.data.isTraversable;
        type = this.data.tileType;
        this.position = position;

        Contents = new TileContent(this);
    }

    public void SetTraversability(bool value) {
        if (IsTraversable != value) {
            IsTraversable = value;
            OnTraversabilityChange?.Invoke(this, new TraversabilityArgs() { IsTraversable = value});
            
            StockpileCreator.RemoveStockpileFromTile(this);//TODO: USE EVENT INSTEAD
        }
    }
}