using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : StaticObject, IEdible, IPlacable {

    #region Components

    public VegetationComponent vegetationComponent { get; protected set; }

    #endregion

    public Grass() : base (Vector2Int.one) {
        isTraversable = true;
        GameManager.GetInstance().natureManager.grassList.Add(this);
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
        GameManager.GetInstance().natureManager.grassList.Remove(this);
        GameManager.GetInstance().natureManager.edibleList.Remove(this);
    }

    #region Vegetation Component

    protected void InitializeVegetationComponent() {
        vegetationComponent = gameObject.AddComponent<VegetationComponent>();
        vegetationComponent.Initialize(this, VegetationType.grass);
    }

    #endregion

    #region IEdible

    public int NutritionValue => 20;

    public Vector2Int GetEdiblePosition() => position;

    public void AddToGlobalEdiblesList() => GameManager.GetInstance().natureManager.edibleList.Add(this);

    #endregion

    #region IPlacable

    public void PutOnTile() => Utils.TileAt(position).contents.PutStaticObjectOnTile(this, isTraversable);
    public void RemoveFromTile() => Utils.TileAt(position).contents.RemoveStaticObjectFromTile();
    
    #endregion  
}