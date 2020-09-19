using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodLog : Item {

    public override string Name => "wood_log";

    protected override int StackCount => 32;

    public WoodLog() {
    }
}