using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public static UIManager Instance;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("Only one UIManager can exist at a time.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void OpenCharacterWindow(Character source) {
        CharacterWindow window = WindowSystem.GetInstance().SwitchWindow(WindowType.CharacterWindow) as CharacterWindow;
        window.character = source;
    }

    public void CloseCharacterWindow() => WindowSystem.GetInstance().FindWindowOfType(WindowType.CharacterWindow)?.CloseWindow();
}