using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Characters/CharacterScriptableObject", order = 1)]
public class CharacterScriptableObject : ScriptableObject
{
    public string name;

    [Range(0, 10)]
    public float movementSpeed;
    public GameObject prefab;

    [Range(0, 10)]
    public float hungerDecreasePerSecond;
}
