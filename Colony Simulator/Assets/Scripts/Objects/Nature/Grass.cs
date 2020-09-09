using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : StaticObject, IEdible
{   
    public override string Name {
        get {
            return "grass";
        }
    }

    public int NutritionValue {
        get {
            return 20;
        }
    }

    public Grass() : base (Vector2Int.one)
    {
        isTraversable = true;
    }

    public void Eat() {

        GameManager.Instance.natureManager.grass.Remove(this);
        GameObject.Destroy(_gameObject);
    }
}
