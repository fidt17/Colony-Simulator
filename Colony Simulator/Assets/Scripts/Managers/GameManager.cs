using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public World world { get; private set; }

    private void Awake() {

        if(Instance != null) {

            Debug.LogError("Only one GameManager can exist at a time!", this);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start() {

        InitializeWorld();
    }

    private void InitializeWorld() {

        Vector2Int dimensions = new Vector2Int(50, 50);

        world = new World(dimensions);
        world.GenerateEmptyWorld();
    }
}
