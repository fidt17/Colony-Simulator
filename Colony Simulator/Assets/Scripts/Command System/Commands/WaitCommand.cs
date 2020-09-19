using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitCommand : Command {
    
    private float _waitTime;

    public WaitCommand(float time) {
        
        _waitTime = time;
    }

    public override void Execute() {

        _waitTime -= Time.deltaTime;

        if (_waitTime <= 0)
            Finish(true);
    }
}
