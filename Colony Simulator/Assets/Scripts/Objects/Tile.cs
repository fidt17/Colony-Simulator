using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {

    empty,
    sand,
    grass,
    water
}

public class Tile : StaticObject
{
    public TileType type { get; protected set; }

    public Tile(Vector2Int pos, GameObject go)
    {
        position = pos;
        dimensions = Vector2Int.one;
        isTraversable = true;
        type = TileType.empty;
        this.gameObject = go;

    }

    public void SetTileType(TileType newType, bool traversable, SpriteRenderer newSR) {

        type = newType;
        isTraversable = traversable;
        ChangeTileSprite(newSR);
    }

    public void ChangeTileSprite(SpriteRenderer newSR) {

        gameObject.GetComponent<SpriteRenderer>().sprite = newSR.sprite;
        gameObject.GetComponent<SpriteRenderer>().color = newSR.color;
    }
}
