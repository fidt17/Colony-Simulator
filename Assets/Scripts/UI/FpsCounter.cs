using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FpsCounter : MonoBehaviour {

    private TextMeshProUGUI fpsCounter = null;

    private void Awake() {
        fpsCounter = GetComponent<TextMeshProUGUI>();
    }

    double frameCount = 0;
    double dt = 0.0;
    double fps = 0.0;
    double updateRate = 4.0;  // 4 updates per sec.
    
    void Update()
    {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0/updateRate)
        {
            fps = frameCount / dt ;
            frameCount = 0;
            dt -= 1.0/updateRate;
        }
        
        fpsCounter.text = "FPS: " + (fps).ToString("0.00");
    }
}
