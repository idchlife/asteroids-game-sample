using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidsCore.Utils {
  public static class DateTimeExtensions {
    public static long ToUnixTimeMs(this DateTime self) {
      return ((DateTimeOffset)self).ToUnixTimeMilliseconds();
    }
  }
}
