using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : StaticObject
{
    public Tile(Vector2Int pos, GameObject gameObject)
    {
        _position = pos;
        _gameObject = gameObject;

        _dimensions = Vector2Int.one;
    }
}
