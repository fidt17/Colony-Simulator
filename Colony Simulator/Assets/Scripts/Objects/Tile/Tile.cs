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

    public TileType type { get; set; }
    public TileContents contents { get; private set; }

    private SpriteRenderer _mainSprite;
    private Color _defaultSpriteColor;

    public override void SetGameObject(GameObject gameObject, Vector2Int position) {
        base.SetGameObject(gameObject, position);
        _mainSprite = gameObject.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        _defaultSpriteColor = _mainSprite.color;
    }

    public override void SetData(PrefabScriptableObject data) {
        base.SetData(data as TileScriptableObject);
        type = ((TileScriptableObject) data).tileType;
        contents = new TileContents(this);
    }

    public void SetTraversability(bool isTraversable) {
        if (this.isTraversable != isTraversable) {
            this.isTraversable = isTraversable;

            PathNode node = Utils.NodeAt(position);
            if (node != null) {
                node.isTraversable = isTraversable;
            }

            GameManager.GetInstance().UpdatePathfinder();
        }
    }

    public void SetSprite(Sprite sprite) => _mainSprite.sprite = sprite;
    public void SetColor(Color color) => _mainSprite.color = color;
    public void ResetColor() => _mainSprite.color = _defaultSpriteColor;
}