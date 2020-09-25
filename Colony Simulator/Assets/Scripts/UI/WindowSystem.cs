﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WindowSystem : MonoBehaviour {

    public static WindowSystem Instance;

    List<WindowComponent> windows;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("Only one WindowSystem can exist at a time!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    
        FindWindows();
        InputController.Instance.OnEscape_Down += CloseWindows;
    }

    private void FindWindows() {
        windows = GetComponentsInChildren<WindowComponent>().ToList();
        CloseWindows();
    }

    public WindowComponent FindWindowOfType(WindowType type) => windows.Find(w => w.windowType == type);

    public WindowComponent SwitchWindow(WindowType type) {
        WindowComponent desiredWindow = FindWindowOfType(type);
        if (desiredWindow != null) {
            if (desiredWindow.gameObject.activeInHierarchy) {
                desiredWindow.CloseWindow();
                return null;
            } else {
                CloseWindows();
                desiredWindow.gameObject.SetActive(true);
                return desiredWindow;
            }
        }
        return null;
    }

    public void SwitchSubWindow(WindowComponent parent, WindowType type) {
        WindowComponent desiredWindow = FindWindowOfType(type);
        if (desiredWindow != null) {
            if (parent.subWindows.Contains(desiredWindow)) {
                desiredWindow.CloseWindow();
                parent.subWindows.Remove(desiredWindow);
            } else {
                foreach(WindowComponent w in parent.subWindows) {
                    w.CloseWindow();
                }
                parent.subWindows = new List<WindowComponent>();
                desiredWindow.gameObject.SetActive(true);
                parent.subWindows.Add(desiredWindow);
            }
        }
    }

    private void CloseWindows() => windows.ForEach(w => w.CloseWindow());

    private void OnDestroy() => InputController.Instance.OnEscape_Down -= CloseWindows;
}