using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEdible {
    int NutritionValue { get; }
    Vector2Int GetEdiblePosition();
    void AddToGlobalEdiblesList();
    void Eat(HungerComponent eater);
}
