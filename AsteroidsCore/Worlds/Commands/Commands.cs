using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace AsteroidsCore.Worlds.Commands {
  public enum CommandsDestination {
    Main,
    Temp,
    Nowhere
  }

  public sealed class Commands {
    public int Size => mainQueue.Count;

    private CommandsDestination destination = CommandsDestination.Main;

    private Queue<Command> mainQueue = new Queue<Command>();

    private Queue<Command> tempQueue = new();

    public void RouteToTemp() {
      destination = CommandsDestination.Temp;
    }

    public void RouteToMain() {
      destination = CommandsDestination.Main;
    }

    public void RouteToNowhere() {
      destination = CommandsDestination.Nowhere;
    }

    public void AddCommand(Command command) {
      if (destination == CommandsDestination.Main) {
        mainQueue.Enqueue(command);
      } else if (destination == CommandsDestination.Temp) {
        tempQueue.Enqueue(command);
      } else {
        // Going straight to /dev/null
      }
    }

    public Command RetrieveAndRemoveCommand() => mainQueue.Dequeue();

    public void TransferAllFromTempToMain() {
      while (tempQueue.Count > 0) mainQueue.Enqueue(tempQueue.Dequeue());
    } 
  }
}
