using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : StaticObject, IPlacable, IHarvestable
{   
    #region Components

    public VegetationComponent vegetationComponent { get; protected set; }

    #endregion

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
        vegetationComponent = gameObject.AddComponent<VegetationComponent>();
        vegetationComponent.Initialize(this, VegetationType.tree);
    }

    #endregion

    #region IPlacable

    public void PutOnTile()      => Utils.TileAt(position).contents.PutStaticObjectOnTile(this, isTraversable);
    public void RemoveFromTile() => Utils.TileAt(position).contents.RemoveStaticObjectFromTile();
    
    #endregion  

    #region IHarvestable

    public void Harvest() {
        Item woodLog = Factory.Create<WoodLog>("wood log", position);
        Destroy();
    }

    #endregion
}