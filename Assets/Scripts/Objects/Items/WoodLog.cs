using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodLog : Item {

    protected override int StackCount => 64;

    #region IPlacable

    public override void AddToRegionContent() => Utils.NodeAt(Position.x, Position.y).subregion?.content.Add<WoodLog>(this);
    protected override void RemoveFromRegionContent() => Utils.NodeAt(Position.x, Position.y).subregion?.content.Remove<WoodLog>(this);
    
    #endregion
}