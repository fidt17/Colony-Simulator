using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {

    empty,
    sand,
    grass,
    water
}

public class Tile : StaticObject {

    public override string Name => "tile";

    public TileType type { get; set; }
    public StaticObject objectOnTile = null;
    public Item itemOnTile = null;

    public SpriteRenderer mainSprite { get; private set; }
    public Color defaultSpriteColor;
    private TileType[,] borderMatrix = new TileType[3,3];

    public Tile() : base (Vector2Int.one) {

        isTraversable = true;
        type = TileType.empty;
    }

    public override void SetGameObject(GameObject gameObject, Vector2Int position) {

        base.SetGameObject(gameObject, position);

        mainSprite = gameObject.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        defaultSpriteColor = mainSprite.color;
    }

    public override void Destroy() {

        Debug.LogWarning("Something went wrong. Tiles are not supposed to be destroyed.");
        base.Destroy();
    }

    public override void SetData(StaticScriptableObject data) {

        base.SetData(data);

        type = ((TileScriptableObject) data).tileType;
        isTraversable = ((TileScriptableObject) data).isTraversable;
    }

    public void PutStaticObjectOnTile(StaticObject staticObject, bool isTraversable) {

        if (objectOnTile != null)
            objectOnTile.Destroy();

        objectOnTile = staticObject;

        if (this.isTraversable != isTraversable)
            GameManager.Instance.UpdatePathfinder();

        this.isTraversable = isTraversable;
    }

    public void RemoveStaticObjectFromTile() {

        if (!isTraversable) {

            GameManager.Instance.UpdatePathfinder();
            isTraversable = true;
        }

        objectOnTile = null;
    }

    public void PutItemOnTile(Item item) {

        if (itemOnTile != null) {
            itemOnTile.Destroy();
            Debug.LogError("Item was destroyed because another item was placed upon it. Fix this.");
            ////////////////////////////////////////////////////////////////////////////////////////////////// FIX ME
        }

        itemOnTile = item;
    }

    public void RemoveItemFromTile() {

        itemOnTile = null;
    }

    //DELETE ME
    public void TestFunc(bool b) {

        isTraversable = b;
    }
    //
}