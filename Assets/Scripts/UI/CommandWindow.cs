using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandWindow : WindowComponent {
    
    public void ClickOnCancelJobCommand() => CommandInputStateMachine.SwitchCommandState(new CancelJobInputState());
    public void ClickOnCutCommand() => CommandInputStateMachine.SwitchCommandState(new CutCommandInputState());
    public void ClickOnBuildCommand(ConstructionScriptableObject construction) => CommandInputStateMachine.SwitchCommandState(new BuildCommandInputState(construction));
    public void ClickOnStockpileCommand() => CommandInputStateMachine.SwitchCommandState(new StockpileState());
    public void ClickOnRemoveStockpileCommand() => CommandInputStateMachine.SwitchCommandState(new RemoveStockpileState());
}
