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

    private SpriteRenderer mainSprite, northBorder, eastBorder, southBorder, westBorder;
    private TileType[,] borderMatrix = new TileType[3,3];

    public Tile(Vector2Int pos, GameObject go)
    {
        position = pos;
        dimensions = Vector2Int.one;
        isTraversable = true;
        type = TileType.empty;
        this.gameObject = go;

        mainSprite = gameObject.transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }

    //DELETE ME
    public void TestFunc(bool b) {

        isTraversable = b;
    }
    //

    public void SetTileType(TileType newType, bool traversable, SpriteRenderer newSR) {

        type = newType;
        isTraversable = traversable;
        ChangeTileSprite(newSR);
    }

    public void ChangeTileSprite(SpriteRenderer newSR) {

        mainSprite.sprite = newSR.sprite;
        mainSprite.color = newSR.color;
    }
}
