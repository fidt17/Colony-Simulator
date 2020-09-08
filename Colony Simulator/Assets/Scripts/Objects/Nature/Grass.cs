using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : StaticObject, IHarvestable
{   
    public override string Name {
        get {
            return "grass";
        }
    }

    public Grass() : base (Vector2Int.one)
    {
        isTraversable = true;
    }

    public Item Harvest() {

        Debug.Log(Name + " was harvested");
        return new Food();
    }
}
