using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using AsteroidsCore.Utils.Math;

namespace AsteroidsCore.Utils.Geometry {
  public struct Vec2 {
    public readonly float X { get; }
    public readonly float Y { get; }

    public Vec2(float x, float y) {
      X = x;
      Y = y;
    }

    public float Magnitude() => MathF.Sqrt(X * X + Y * Y);

    public Vec2 Normalized() {
      var magnitude = Magnitude();

      if (magnitude == 0) return this;

      return this / magnitude;
    }

    public float DistanceTo(Vec2 target) =>
      MathF.Sqrt(MathF.Pow(target.X - X, 2) + MathF.Pow(target.Y - Y, 2));

    public float DirectionToRadians() => MathF.Atan2(Y, X);

    public static Vec2 Zero {
      get => new(0, 0);
    }

    public static bool operator ==(Vec2 a, Vec2 b) =>
      a.X == b.X && a.Y == b.Y;
    public static bool operator !=(Vec2 a, Vec2 b) =>
      (a == b) == false;

    public static Vec2 operator -(Vec2 a, Vec2 b) =>
      new(a.X - b.X, a.Y - b.Y);

    public static Vec2 operator +(Vec2 a, Vec2 b) =>
      new(a.X + b.X, a.Y + b.Y);

    public static Vec2 operator /(Vec2 a, float b) =>
      new(a.X / b, a.Y / b);

    public static Vec2 operator *(Vec2 a, float b) =>
      new(a.X * b, a.Y * b);

    public static Vec2 Lerp(Vec2 a, Vec2 b, float weight) {
      return new Vec2(
        MathUtils.Lerp(a.X, b.X, weight),
        MathUtils.Lerp(a.Y, b.Y, weight)
      );
    }

    public static Vec2 DirectionFromPointToPoint(Vec2 a, Vec2 b) {
      return (b - a).Normalized();
    }

    public static Vec2 DirectionFromRadians(double radians) =>
      new Vec2(MathF.Cos((float) radians), MathF.Sin((float) radians)).Normalized();

    public override string ToString() => $"|{X}:{Y}|";
  }
}
