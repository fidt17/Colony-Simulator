using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VegetationType {
    grass,
    tree
}

public class VegetationComponent : MonoBehaviour {

    public StaticObject   vegetation { get; private set; }
    public VegetationType type       { get; private set; }
    
    public void Initialize(StaticObject vegetation, VegetationType type) {
        this.vegetation = vegetation;
        this.type = type;
    }
}