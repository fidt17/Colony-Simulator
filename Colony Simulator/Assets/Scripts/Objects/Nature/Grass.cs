using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : StaticObject, IEdible, IPlacable {

    public override string Name => "grass";

    #region Components

    public VegetationComponent vegetationComponent { get; protected set; }

    #endregion

    public Grass() : base (Vector2Int.one) {
        isTraversable = true;
        GameManager.Instance.natureManager.grassList.Add(this);
        AddToGlobalEdiblesList();
    }

    public override void SetGameObject(GameObject gameObject, Vector2Int position) {
        base.SetGameObject(gameObject, position);
        InitializeVegetationComponent();
        PutOnTile();
    }

    public override void Destroy() {
        base.Destroy();
        RemoveFromTile();
        GameManager.Instance.natureManager.grassList.Remove(this);
        GameManager.Instance.natureManager.edibleList.Remove(this);
    }

    #region Vegetation Component

    protected void InitializeVegetationComponent() {
        vegetationComponent = _gameObject.AddComponent<VegetationComponent>();
        vegetationComponent.Initialize(this, VegetationType.grass);
    }

    #endregion

    #region IEdible

    public int NutritionValue => 20;

    public Vector2Int GetEdiblePosition() => position;

    public void AddToGlobalEdiblesList() => GameManager.Instance.natureManager.edibleList.Add(this);

    public void Eat(HungerComponent eater) {
        eater.ChangeHunger(NutritionValue);
        Destroy();        
    }

    #endregion

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
}