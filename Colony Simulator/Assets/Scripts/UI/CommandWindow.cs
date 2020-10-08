using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandWindow : WindowComponent {
    
    public void ClickOnCutCommand() => CommandInputStateMachine.SwitchCommandState(new CutCommandInputState());
    public void ClickOnStockpileCommand() => CommandInputStateMachine.SwitchCommandState(new StockpileState());
}
