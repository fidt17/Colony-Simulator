using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tree : Vegetation {
    
    public override Item Cut() {
        base.Cut();
        return Factory.Create<WoodLog>("wood log", Position);
    }
}