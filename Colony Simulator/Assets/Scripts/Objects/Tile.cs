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
    public TileType _type { get; protected set; }

    public Tile(Vector2Int pos, GameObject gameObject)
    {
        _position = pos;
        _dimensions = Vector2Int.one;
        _isTraversable = true;
        _type = TileType.empty;
        _gameObject = gameObject;

    }

    public void SetTileType(TileType newType, bool traversable, SpriteRenderer newSR) {

        _type = newType;
        _isTraversable = traversable;
        ChangeTileSprite(newSR);
    }

    public void ChangeTileSprite(SpriteRenderer newSR) {

        _gameObject.GetComponent<SpriteRenderer>().sprite = newSR.sprite;
        _gameObject.GetComponent<SpriteRenderer>().color = newSR.color;
    }
}
