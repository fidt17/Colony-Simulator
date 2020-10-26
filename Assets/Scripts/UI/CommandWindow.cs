using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandWindow : WindowComponent {
    
    public void ClickOnCutCommand() => CommandInputStateMachine.SwitchCommandState(new CutCommandInputState());
    public void ClickOnBuildWallCommand() => CommandInputStateMachine.SwitchCommandState(new BuildCommandInputState());
    public void ClickOnStockpileCommand() => CommandInputStateMachine.SwitchCommandState(new StockpileState());
    public void ClickOnRemoveStockpileCommand() => CommandInputStateMachine.SwitchCommandState(new RemoveStockpileState());
}
