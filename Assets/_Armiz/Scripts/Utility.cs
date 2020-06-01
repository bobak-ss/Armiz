using System;
using UnityEngine;

public class Utility
{
    public static float rCos(float r, float angle)
    {
        return r * Mathf.Cos(angle * Mathf.Deg2Rad);
    }

    public static float rSin(float r, float angle)
    {
        return r * Mathf.Sin(angle * Mathf.Deg2Rad);
    }
}
