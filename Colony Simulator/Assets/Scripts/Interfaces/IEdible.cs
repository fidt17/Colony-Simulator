using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEdible
{   
    float NutritionValue { get; }
    void Eat();
}
