using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "JobIcons/JobIconScriptableObject", order = 1)]
public class JobIconScriptableObject : ScriptableObject {
    public string name;
    public GameObject prefab;
}
