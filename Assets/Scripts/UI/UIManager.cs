using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager> {

    private void Start() {
        InputListener.GetInstance().OnTab_Up += SwitchCommandWindow;
    }

    public void OpenCharacterWindow(Character source) {
        CharacterWindow window = WindowSystem.GetInstance().SwitchWindow(WindowType.CharacterWindow) as CharacterWindow;
        window.character = source;
    }

    public void CloseCharacterWindow() => WindowSystem.GetInstance().FindWindowOfType(WindowType.CharacterWindow)?.CloseWindow();

    private void SwitchCommandWindow() => WindowSystem.GetInstance().SwitchWindow(WindowType.CommandWindow);
}