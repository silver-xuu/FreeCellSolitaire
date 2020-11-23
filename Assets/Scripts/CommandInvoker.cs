using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandInvoker : MonoBehaviour
{
    static Queue<ICommand> commandBuffer;

    static List<ICommand> commandHistory;

    static int counter;

    public Text logText;
    private void Awake()
    {
        commandBuffer = new Queue<ICommand>();
        commandHistory = new List<ICommand>();
        counter = 0;
    }
    public static void AddCommand(ICommand command)
    {
        if (counter < commandHistory.Count)
        {
            while (commandHistory.Count > counter)
            {
                commandHistory.RemoveAt(counter);
            }
        }
        commandBuffer.Enqueue(command);
    }

    // Update is called once per frame
    void Update()
    {
        if (commandBuffer.Count > 0)
        {
            ICommand command = commandBuffer.Dequeue();
            command.Execute();

            commandHistory.Add(command);
            counter++;
            logText.text = "Log:\t" + command.ToString();
        }
        
    }
    public void UndoCommand()
    {
        if (commandBuffer.Count <= 0 && counter>0)
        {
            counter--;
            ICommand c = commandHistory[counter];
            c.UndoAction();
            logText.text = "Log:\tUndo " + c.ToString();
        }
    }
    public void RedoCommand()
    {
        if (commandBuffer.Count <= 0 && commandHistory.Count > 0 &&commandHistory.Count>counter)
        {
            ICommand c = commandHistory[counter];
            c.Execute();
            counter++;
            logText.text = "Log:\tRedo " + c.ToString();
        }
    }
}
