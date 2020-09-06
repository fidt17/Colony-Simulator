using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class WindowSwitcher : MonoBehaviour
{
    public WindowType windowType;
    public bool isSubWindow = false;

    private Button button;

    private void Start() {

        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);    
    }

    private void OnButtonClick() {
        
        if(!isSubWindow)
            WindowSystem.Instance.SwitchWindow(windowType);
        else
            WindowSystem.Instance.SwitchSubWindow(transform.parent.GetComponent<WindowComponent>(), windowType);
    }
}
