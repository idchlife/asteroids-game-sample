using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Utils.State {
  public interface IStateful {
  }

  public interface IStateful<TState> where TState : struct {
    public void SetState(TState state);
    public TState GetState();
  }
}
