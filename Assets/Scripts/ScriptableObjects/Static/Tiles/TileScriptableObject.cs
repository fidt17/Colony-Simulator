using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Static/Tiles/TileScriptableObject", order = 1)]
public class TileScriptableObject : StaticScriptableObject {
    public TileType tileType;
    public Color defaultColor = Color.white;
}

public enum TileType {
    empty,
    sand,
    grass,
    water
}
