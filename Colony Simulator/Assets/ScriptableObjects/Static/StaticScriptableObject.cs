using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Static/StaticScriptableObject", order = 1)]
public class StaticScriptableObject : ScriptableObject
{
    public new string name;
    public GameObject prefab;
}
