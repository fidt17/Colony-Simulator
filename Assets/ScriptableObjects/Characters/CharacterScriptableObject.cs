using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Characters/CharacterScriptableObject", order = 1)]
public class CharacterScriptableObject : PrefabScriptableObject {
    [Range(0, 10)]
    public float movementSpeed;
    [Range(0, 10)]
    public float hungerDecreasePerSecond;
    [Range(10, 90)]
    public float hungerSearchThreshold = 80;
}
