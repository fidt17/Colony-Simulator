using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ingredient {
    public string itemName;
    public int count;
}

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Static/Constructions/StaticScriptableObject", order = 1)]
public class ConstructionScriptableObject : ScriptableObject {
    public GameObject constructionPrefab; 
    public GameObject planPrefab;
    public List<Ingredient> ingredients;
}
