using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {
    
    public static CursorManager Instance;

    public Texture2D defaultTexture;
    public Texture2D moveStateTexture;
    public Texture2D cutStateTexture;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("Only one CursorManager can exist at a time.");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SwitchTexture(defaultTexture);
    }

    public void SwitchTexture(Texture2D texture) => Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
}