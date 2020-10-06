using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : StaticObject, IPlacable, IHarvestable
{   
    #region Components

    public VegetationComponent vegetationComponent { get; protected set; }

    #endregion

    public Tree() : base (Vector2Int.one) {
        isTraversable = false;
    }

    public override void SetGameObject(GameObject gameObject, Vector2Int position) {
        base.SetGameObject(gameObject, position);
        InitializeVegetationComponent();
        PutOnTile();
    }

    public override void Destroy() {
        base.Destroy();
        RemoveFromTile();
    }

    #region Vegetation Component

    protected void InitializeVegetationComponent() {
        vegetationComponent = _gameObject.AddComponent<VegetationComponent>();
        vegetationComponent.Initialize(this, VegetationType.tree);
    }

    #endregion

    #region IPlacable

    public void PutOnTile() => GameManager.Instance.world.GetTileAt(position).PutStaticObjectOnTile(this, isTraversable);
    public void RemoveFromTile() => GameManager.Instance.world.GetTileAt(position).RemoveStaticObjectFromTile();
    
    #endregion  

    #region IHarvestable

    public void Harvest() {
        Factory.Create<WoodLog>("wood log", position);
        Destroy();
    }

    #endregion
}