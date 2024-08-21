using System;
using System.Collections.Concurrent;
using UnityEngine;

public class UnityMainThreadExecutor : MonoBehaviour {
  private const int MAX_ITEMS_PER_UPDATE = 10;

  private ConcurrentQueue<Action> actions { get; set; } = new();

  public void AddActionToQueue(Action action) {
    actions.Enqueue(action);
  }

  private void Update() {
    var i = 0;

    while (actions.TryDequeue(out var action) && i < MAX_ITEMS_PER_UPDATE) {
      action.Invoke();

      i++;
    }
  }
}