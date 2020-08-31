using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StaticObject
{
    public Vector2Int _position   { get; protected set; }
    public Vector2Int _dimensions { get; protected set; }
    public GameObject _gameObject { get; protected set; }
}
