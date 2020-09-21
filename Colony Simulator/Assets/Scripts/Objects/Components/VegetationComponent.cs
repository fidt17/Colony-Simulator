using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VegetationType {

    grass,
    tree
}

public class VegetationComponent : MonoBehaviour {

    public VegetationType Type => _type;
    public StaticObject Vegetation => _vegetation;

    private StaticObject _vegetation;
    private VegetationType _type;
    
    public void Initialize(StaticObject vegetation, VegetationType type) {

        _vegetation = vegetation;
        _type = type;
    }
}
