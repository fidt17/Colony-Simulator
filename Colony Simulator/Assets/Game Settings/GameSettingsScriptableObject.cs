﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapSize {
    small  = 50,
    medium = 75,
    large  = 100
}

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Game Settings/Settings", order = 1)]
public class GameSettingsScriptableObject : ScriptableObject
{
    [Header("World Generation")]

    public MapSize mapSize;

    public int seed = 12;

    [Header("Characters")]

    [Range(0, 40)]
    public int humanCount;
    [Range(0, 200)]
    public int rabbitCount;
}
