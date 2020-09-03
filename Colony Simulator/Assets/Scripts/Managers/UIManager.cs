using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI tileCoordsTMP;


    private void Awake() {

        if (Instance != null) {

            Debug.LogError("Only one UIManager can exist at a time!");
            Destroy(gameObject);
        }

        Instance = this;
    }

    public void SetTileCoords(Vector2Int coords) {

        tileCoordsTMP.text = coords.x + " " + coords.y;
    }
}
