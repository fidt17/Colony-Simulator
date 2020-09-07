using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabStorage : MonoBehaviour
{
    public static PrefabStorage Instance;

    private void Awake() {

        if(Instance != null) {

            Debug.LogError("Only one PrefabStorage can exist at a time!", this);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    [Header("Tile prefabs")]
    public GameObject protoTile;
    public GameObject waterTile;
    public GameObject greenBrickTile, redBrickTile;
    public GameObject sandTile;
    public GameObject grassTile;
}
