using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public World world { get; private set; }
    public Pathfinder pathfinder { get; private set; }
    public CharacterManager characterManager { get; private set; }
    
    //TODO
    //Create startup settings system
    private Vector2Int _dimensions = new Vector2Int(100, 100);


    private void Awake() {

        if(Instance != null) {

            Debug.LogError("Only one GameManager can exist at a time!", this);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start() {

        Initialize();
    }

    #region Initialization

    private void Initialize() {


        CreateWorld();
        CreatePathfinder();
        CreateCharacters();
        Camera.main.GetComponent<CameraController>().Init();
    }

    private void CreateWorld() {


        world = new World(_dimensions);

        WorldGenerator wg = new WorldGenerator();
        wg.GenerateTerrainWithPerlinNoise(_dimensions, ref world.grid);
    }

    private void CreatePathfinder() {

        if (world == null) {
            
            Debug.LogError("Cannot create Pathfinder because the world is not yet created.");
            return;
        }

        pathfinder = new Pathfinder(world.dimensions);
        pathfinder.CreateRegionSystem();
    }

    private void CreateCharacters() {

        characterManager = new CharacterManager();
        characterManager.CreateInitialCharacters();
    }

    #endregion
}
