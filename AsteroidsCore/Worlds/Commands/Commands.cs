using System;
using System.Collections.Concurrent;
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

    private ConcurrentQueue<Command> mainQueue = new();

    private ConcurrentQueue<Command> tempQueue = new();

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
    
    // Concurrent variant
    public bool TryRetrieveAndRemoveCommand(out Command? command) {
      if (mainQueue.TryDequeue(out var c)) {
        command = c;
        return true;
      } else {
        command = null;
        return false;
      }
    }

    public Command? RetrieveAndRemoveCommand() {
      if (mainQueue.TryDequeue(out var command)) return command;

      return null;
    }

    public void TransferAllFromTempToMain() {
      while (tempQueue.Count > 0) {
        if (!tempQueue.TryDequeue(out var command)) {
          mainQueue.Enqueue(command);
        }
      }
    } 
  }
}
