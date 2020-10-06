using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Items/ItemScriptableObject", order = 1)]
public class ItemScriptableObject : ScriptableObject {
    public new string name;
    public GameObject prefab;
}
