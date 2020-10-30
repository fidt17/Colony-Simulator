using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Vegetation
{   
    #region ICuttable

    public override Item Cut() {
        base.Cut();
        return Factory.Create<WoodLog>("wood log", position);
    }

    #endregion
}