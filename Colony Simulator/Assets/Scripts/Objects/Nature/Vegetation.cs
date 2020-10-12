using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegetation : StaticObject, IHarvestable
{   
    #region IHarvestable

    public virtual void Harvest() => Destroy();

    #endregion
}