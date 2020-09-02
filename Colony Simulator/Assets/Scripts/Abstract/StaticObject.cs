using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StaticObject
{
    public Vector2Int position   { get; protected set; }
    public Vector2Int dimensions { get; protected set; }
    public GameObject gameObject { get; protected set; }

    public bool isTraversable { get; protected set; }
    public void DestroyGO() {

        GameObject.Destroy(gameObject);
    }
}
