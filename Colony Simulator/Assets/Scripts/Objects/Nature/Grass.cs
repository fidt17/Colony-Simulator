using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : StaticObject, IEdible, IPlacable
{   
    public override string Name {
        get {
            return "grass";
        }
    }

    public Grass() : base (Vector2Int.one)
    {
        isTraversable = true;
        GameManager.Instance.natureManager.grassList.Add(this);

        AddToGlobalEdiblesList();
    }

    public override void SetGameObject(GameObject gameObject, Vector2Int position) {

        base.SetGameObject(gameObject, position);
        PutOnTile();
    }

    public override void Destroy() {

        base.Destroy();
        GameManager.Instance.natureManager.grassList.Remove(this);
        GameManager.Instance.natureManager.edibleList.Remove(this);
    }

    #region IEdible

    public int NutritionValue {
        get {
            return 20;
        }
    }

    public Vector2Int GetEdiblePosition() {

        return position;
    }

    public void AddToGlobalEdiblesList() {

        GameManager.Instance.natureManager.edibleList.Add(this);
    }

    public void Eat(HungerComponent eater) {

        eater.ChangeHunger(NutritionValue);
        Destroy();        
    }

    #endregion

    #region IPlacable

    public void PutOnTile() {

        Tile tile = GameManager.Instance.world.GetTileAt(position);

        tile.objectOnTile?.Destroy();
        tile.objectOnTile = this;
    }

    public void RemoveFromTile() {

        Tile tile = GameManager.Instance.world.GetTileAt(position);

        tile.objectOnTile = null;
    }
    
    #endregion  
}
