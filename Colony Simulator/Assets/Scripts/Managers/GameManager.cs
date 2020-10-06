﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    public World world { get; private set; }
    public CharacterManager characterManager { get; private set; }
    public NatureManager natureManager { get; private set; }

    public GameSettingsScriptableObject gameSettings;

    protected override void Awake() {
        characterManager = GetComponent<CharacterManager>();
        natureManager = GetComponent<NatureManager>();
    }

    private void Start() => Initialize();

    #region Initialization

    private void Initialize() {
        CreateWorld();
        Camera.main.GetComponent<CameraController>().Init();
        CommandInputStateMachine.Initialize();
    }

    private void CreateWorld() {
        var dimensions = new Vector2Int(gameSettings.worldWidth, gameSettings.worldHeight);
        world = new World(dimensions);
        WorldGenerator.GenerateWorld(gameSettings, ref world.grid);
    }

    public void UpdatePathfinder() => StartCoroutine(Pathfinder.UpdateSystem());

    #endregion
}