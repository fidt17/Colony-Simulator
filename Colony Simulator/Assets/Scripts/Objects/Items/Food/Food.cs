using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Food : Item, IEdible
{   
    public override string Name {
        get {
            return "food";
        }
    }

    public virtual int NutritionValue {
        get {
            return 1;
        }
    }

    public void Eat() {
        Debug.Log(Name + " was eaten.");
    }
}
