using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandProcessor : MonoBehaviour
{
    public Queue<Command> commandList = new Queue<Command>();
    private Command currentCommand = null;
    private bool isRunning = false;

    public void AddCommand(Command command) {

        commandList.Enqueue(command);
    }

    public void ExecuteNextCommand() {

        currentCommand = null;

        if (commandList.Count == 0)
            return;

        isRunning = true;
        currentCommand = commandList.Dequeue();
    }

    public void ResetCommands() {

        commandList = new Queue<Command>();
        isRunning = false;
    }

    private void ExecuteCurrentCommand() {

        if (currentCommand == null)
            return;

        if (currentCommand.inProgress == false)
            currentCommand.CommandResultHandler += FinishCommand;

        currentCommand.Execute();
    }

    private void FinishCommand(bool succeed) {

        currentCommand.CommandResultHandler -= FinishCommand;

        if (succeed)
            ExecuteNextCommand();
        else
            ResetCommands();
    }

    private void Update() {

        if (isRunning)
            ExecuteCurrentCommand();
    }

}
