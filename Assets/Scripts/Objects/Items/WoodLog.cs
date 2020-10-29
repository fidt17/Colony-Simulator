using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodLog : Item {

    protected override int StackCount => 64;

    #region IPlacable

    public override void AddToRegionContent() => Utils.NodeAt(position.x, position.y).subregion?.content.Add<WoodLog>(this);
    public override void RemoveFromRegionContent() => Utils.NodeAt(position.x, position.y).subregion?.content.Remove<WoodLog>(this);
    
    #endregion
}