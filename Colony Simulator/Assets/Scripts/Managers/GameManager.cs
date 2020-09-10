using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public World world { get; private set; }
    public Pathfinder pathfinder { get; private set; }
    public CharacterManager characterManager { get; private set; }
    public NatureManager natureManager { get; private set; }
    
    //TODO
    //Create startup settings system
    private Vector2Int _dimensions = new Vector2Int(50, 50);

    private void Awake() {

        if(Instance != null) {

            Debug.LogError("Only one GameManager can exist at a time!", this);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        characterManager = GetComponent<CharacterManager>();
        natureManager = GetComponent<NatureManager>();
    }

    private void Start() {

        Initialize();
    }

    #region Initialization

    private void Initialize() {

        CreateWorld();
        CreatePathfinder();
        Camera.main.GetComponent<CameraController>().Init();
    }

    private void CreateWorld() {

        world = new World(_dimensions);
        WorldGenerator.GenerateWorld(_dimensions, ref world.grid);
    }

    private void CreatePathfinder() {

        if (world == null) {
            
            Debug.LogError("Cannot create Pathfinder because the world is not yet created.");
            return;
        }

        pathfinder = new Pathfinder(world.dimensions);
        pathfinder.CreateRegionSystem();
    }

    #endregion
}
