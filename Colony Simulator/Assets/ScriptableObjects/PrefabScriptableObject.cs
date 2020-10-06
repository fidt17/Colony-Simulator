using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "PrefabScriptableObject", order = 2)]
public class PrefabScriptableObject : ScriptableObject {
    public string dataName;
    public GameObject prefab;
}
