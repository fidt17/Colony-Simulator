using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Game Settings/Settings", order = 1)]
public class GameSettingsScriptableObject : ScriptableObject
{
    [Header("World Generation")]

    [Range(20, 100)]
    public int worldWidth = 50;
    [Range(20, 100)]
    public int worldHeight = 50;

    public int seed = 12;

    [Header("Characters")]

    [Range(0, 1)]
    public int humanCount;
    [Range(0, 200)]
    public int rabbitCount;
}
