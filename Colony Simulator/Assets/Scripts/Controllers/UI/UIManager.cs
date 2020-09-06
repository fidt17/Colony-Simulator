using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake() {

        if (Instance != null) {

            Debug.LogError("Only one UIManager can exist at a time!");
            Destroy(gameObject);
        }

        Instance = this;
    }
}
