using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Game.Controls {
  public class GameControls {
    public event EventHandler? ForwardPressed;
    public event EventHandler? ForwardReleased;

    public event EventHandler? LeftPressed;
    public event EventHandler? LeftReleased;

    public event EventHandler? RightPressed;
    public event EventHandler? RightReleased;

    public event EventHandler? ProjectilePressedAndReleased;
    public event EventHandler? LaserPressedAndReleased;

    public void PressForward() => ForwardPressed?.Invoke(this, null);
    public void ReleaseForward() => ForwardReleased?.Invoke(this, null);

    public void PressLeft() => LeftPressed?.Invoke(this, null);
    public void ReleaseLeft() => LeftReleased?.Invoke(this, null);

    public void PressRight() => RightPressed?.Invoke(this, null);
    public void ReleaseRight() => RightReleased?.Invoke(this, null);

    public void PressAndReleaseProjectile() => ProjectilePressedAndReleased?.Invoke(this, null);
    public void PressAndReleaseLaser() => LaserPressedAndReleased?.Invoke(this, null);
  }
}
