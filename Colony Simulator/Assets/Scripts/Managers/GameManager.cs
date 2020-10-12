using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    public World world { get; private set; }
    public CharacterManager characterManager { get; private set; }

    public GameSettingsScriptableObject gameSettings;

    protected override void Awake() {
        characterManager = GetComponent<CharacterManager>();
    }

    private void Start() => Initialize();

    #region Initialization

    private void Initialize() {
        CreateWorld();
        Camera.main.GetComponent<CameraController>().Init();
        CommandInputStateMachine.Initialize();
    }

    private void CreateWorld() {
        world = new World();
        WorldGenerator.GenerateWorld(gameSettings, ref world.grid);
    }

    #endregion
}