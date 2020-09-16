using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {
    
    public Texture2D cursorTex;
    public int cursorSize = 63;
    
    private int _sizeX;
    private int _sizeY;
 
    void Awake() {
        _sizeX = cursorSize;
        _sizeY = cursorSize;
        Cursor.visible = false;
    }

    private void Update() {

        _sizeX = cursorSize;
        _sizeY = cursorSize;
    }
 
    private void OnGUI() {
        GUI.DrawTexture(new Rect(Event.current.mousePosition.x - (cursorSize/2),
                        Event.current.mousePosition.y - (cursorSize/2),
                        _sizeX,
                        _sizeY),
                        cursorTex);
    }
}
