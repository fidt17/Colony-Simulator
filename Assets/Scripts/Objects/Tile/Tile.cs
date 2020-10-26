using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {

    empty,
    sand,
    grass,
    water
}

public class Tile : IData {

    public TileType type { get; set; }
    public TileContent content { get; private set; }

    public TileScriptableObject data { get; protected set; }
    public Sprite GetSprite() => data.prefabSprite;
    public Color GetColor() => data.defaultColor;

    public Vector2Int position   { get; protected set; }
    public bool isTraversable    { get; protected set; }

    public void SetData(PrefabScriptableObject data, Vector2Int position) {
        this.data = data as TileScriptableObject;
        isTraversable = this.data.isTraversable;
        type = this.data.tileType;
        this.position = position;

        content = new TileContent(this);
    }

    public void SetTraversability(bool isTraversable) {
        if (this.isTraversable != isTraversable) {
            this.isTraversable = isTraversable;

            PathNode node = Utils.NodeAt(position);
            if (node != null) {
                node.isTraversable = isTraversable;
            }

            Pathfinder.UpdateSystemAt(position.x, position.y);
            StockpileCreator.RemoveStockpileFromTile(this);
        }
    }
}