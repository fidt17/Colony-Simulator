using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : Vegetation, IEdible {

    #region IEdible

    public int NutritionValue => 20;
    public Vector2Int GetEdiblePosition() => position;

    #endregion

    #region IPlacable

    public override void AddToRegionContent() => Utils.NodeAt(position.x, position.y).subregion.content.Add<Grass>(this);
    public override void RemoveFromRegionContent() => Utils.NodeAt(position.x, position.y).subregion.content.Remove<Grass>(this);
    
    #endregion  
}