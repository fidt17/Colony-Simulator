using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {

    empty,
    sand,
    grass,
    water
}

public class Tile : IPrefab {

    public TileType type { get; set; }
    public TileContent content { get; private set; }

    private SpriteRenderer _mainSprite;
    private Color _defaultSpriteColor;

    public TileScriptableObject data { get; protected set; }
    public GameObject gameObject       { get; protected set; }

    public Vector2Int position   { get; protected set; }
    public bool isTraversable    { get; protected set; }

    public SelectableComponent selectableComponent { get; protected set; }

    #region IPrefab

    public void SetGameObject(GameObject gameObject) {
        this.gameObject = gameObject;
        this.gameObject.transform.position = new Vector3(position.x, position.y, 0);
        _mainSprite = gameObject.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        _defaultSpriteColor = _mainSprite.color;
    }

    public void SetData(PrefabScriptableObject data, Vector2Int position) {
        this.data = data as TileScriptableObject;
        isTraversable = this.data.isTraversable;
        type = this.data.tileType;
        this.position = position;

        content = new TileContent(this);
    }

    public virtual void Destroy() {
        GameObject.Destroy(gameObject);
        Debug.Log("Something went wrong. Tiles are not supposed to be destroyed. " + position);
    }

    #endregion

    public void SetTraversability(bool isTraversable) {
        if (this.isTraversable != isTraversable) {
            this.isTraversable = isTraversable;

            PathNode node = Utils.NodeAt(position);
            if (node != null) {
                node.isTraversable = isTraversable;
            }

            Pathfinder.UpdateSystemAt(position.x, position.y);
        }
    }

    public void SetSprite(Sprite sprite) => _mainSprite.sprite = sprite;
    public void SetColor(Color color) => _mainSprite.color = color;
    public void ResetColor() => _mainSprite.color = _defaultSpriteColor;
}