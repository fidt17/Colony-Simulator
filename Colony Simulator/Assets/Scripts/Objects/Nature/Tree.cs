using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : StaticObject, IPlacable, IHarvestable
{   
    public override string Name => "tree";

    public Tree() : base (Vector2Int.one) {
        isTraversable = false;
    }

    public override void SetGameObject(GameObject gameObject, Vector2Int position) {

        base.SetGameObject(gameObject, position);

        //SpriteRenderer treeSprite = gameObject.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        //treeSprite.sortingOrder = -position.y;
        PutOnTile();
    }

    public override void Destroy() {

        base.Destroy();
        RemoveFromTile();
    }

    #region IPlacable

    public void PutOnTile() {

        Tile tile = GameManager.Instance.world.GetTileAt(position);
        tile.PutStaticObjectOnTile(this, isTraversable);
    }

    public void RemoveFromTile() {

        Tile tile = GameManager.Instance.world.GetTileAt(position);
        tile.RemoveStaticObjectFromTile();
    }
    
    #endregion  

    #region IHarvestable

    public void Harvest() {

        ItemSpawnFactory.GetNewItem("wood_log", "wood_log", position);
        Destroy();
    }

    #endregion
}