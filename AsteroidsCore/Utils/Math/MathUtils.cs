using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Utils.Math {
  public static class MathUtils {
    public static float Lerp(float a, float b, float weight) {
      weight = MathF.Max(0, MathF.Min(1, weight));

      return a + (b - a) * weight;
    }

    public static double Lerp(double a, double b, float weight) {
      weight = MathF.Max(0, MathF.Min(1, weight));

      return a + (b - a) * weight;
    }
  }
}
