using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapSize {
    small  = 100,
    medium = 150,
    large  = 200
}

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Game Settings/Settings", order = 1)]
public class GameSettingsScriptableObject : ScriptableObject
{   
    public int targetFrameRate = 60;

    [Range(1, 2)]
    public int gameSpeed = 1;

    [Header("World Generation")]

    public MapSize mapSize;

    public int seed = 12;

    public bool vegetation = true;
    
    [Header("Characters")]

    [Range(0, 40)]
    public int humanCount;
    [Range(0, 200)]
    public int rabbitCount;

    [Header("Testing")]
    public bool testWorld = false;
}
