using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    public World world { get; private set; }
    public Pathfinder pathfinder { get; private set; }
    public CharacterManager characterManager { get; private set; }
    public NatureManager natureManager { get; private set; }

    public GameSettingsScriptableObject gameSettings;

    private void Awake() {

        if(Instance != null) {

            Debug.LogError("Only one GameManager can exist at a time", this);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        characterManager = GetComponent<CharacterManager>();
        natureManager = GetComponent<NatureManager>();
    }

    private void Start() => Initialize();

    #region Initialization

    private void Initialize() {

        CreateWorld();
        Camera.main.GetComponent<CameraController>().Init();
    }

    private void CreateWorld() {

        var dimensions = new Vector2Int(gameSettings.worldWidth, gameSettings.worldHeight);

        world = new World(dimensions);
        WorldGenerator.GenerateWorld(gameSettings, ref world.grid);
        CreatePathfinder();
    }

    private void CreatePathfinder() {

        if (world == null) {
            
            Debug.LogError("Cannot create Pathfinder because the world is not yet created.");
            return;
        }

        pathfinder = new Pathfinder(world.dimensions);
        pathfinder.CreateRegionSystem();
    }

    public void UpdatePathfinder() {

        if(pathfinder == null)
            return;

        StartCoroutine(pathfinder.UpdateSystem());
    }

    #endregion
}
