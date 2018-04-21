using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Easings
{
  public static float EaseOutInCubic(float t, float b, float c, float d = 1.0f)
  {
    var ts = (t /= d) * t;
    var tc = ts * t;
    return b + c * (6 * tc * ts + -15 * ts * ts + 10 * tc);
  }
}
