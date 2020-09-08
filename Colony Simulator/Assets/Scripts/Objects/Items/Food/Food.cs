using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item, IEdible
{   
    public override string Name {
        get {
            return "food";
        }
    }

    public virtual float NutritionValue {
        get {
            return 1f;
        }
    }

    public void Eat() {
        Debug.Log(Name + " was eaten.");
    }
}
