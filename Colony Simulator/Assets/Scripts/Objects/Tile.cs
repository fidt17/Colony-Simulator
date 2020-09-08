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
    public TileType type { get; set; }

    public override string Name {
        get {
            return "tile";
        }
    }

    public SpriteRenderer mainSprite { get; private set; }
    public Color defaultSpriteColor;

    private TileType[,] borderMatrix = new TileType[3,3];

    public Tile() : base (Vector2Int.one)
    {
        isTraversable = true;
        type = TileType.empty;
    }

    public override void SetGameObject(GameObject gameObject, Vector2Int position) {

        base.SetGameObject(gameObject, position);

        mainSprite = gameObject.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        defaultSpriteColor = mainSprite.color;
    }

    public override void SetData(StaticScriptableObject data) {

        base.SetData(data);

        type = ((TileScriptableObject) data).tileType;
        isTraversable = ((TileScriptableObject) data).isTraversable;
    }

    //DELETE ME
    public void TestFunc(bool b) {

        isTraversable = b;
    }
    //
}
