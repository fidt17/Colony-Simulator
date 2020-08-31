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
    

    public GameObject protoTile;
}
