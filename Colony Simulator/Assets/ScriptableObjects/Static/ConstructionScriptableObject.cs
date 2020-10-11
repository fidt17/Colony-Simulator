using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Ingredient {
    public string itemName;
    public int count;
}

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Static/Constructions/StaticScriptableObject", order = 1)]
public class ConstructionScriptableObject : StaticScriptableObject {
    public List<Ingredient> ingredients;
}
