using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Vegetation
{   

    #region IHarvestable

    public override void Harvest() {
        base.Harvest();
        Factory.Create<WoodLog>("wood log", position);
    }

    #endregion
}