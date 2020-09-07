using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Characters/CharacterScriptableObject", order = 1)]
public class CharacterScriptableObject : ScriptableObject
{
    public string name;

    public float movementSpeed;
    public GameObject prefab;
}
