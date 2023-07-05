using System;
using UnityEngine;

public static class FloatExtensions
{
    private const float EQUALS_TOLERANCE = 0.001f;

    public static float RoundToNearestTen(this float number)
    {
        var x = Mathf.Round(number);
        var y = x / 10;
        var z = Mathf.Round(y);

        return z * 10;
    }

    public static bool EqualsWithTolerance(this float number, float otherNumber)
    {
        return Math.Abs(number - otherNumber) < EQUALS_TOLERANCE;
    }

    public static bool EqualsWithTolerance(this float number, float otherNumber, float equalsTolerance)
    {
        return Math.Abs(number - otherNumber) < equalsTolerance;
    }

    public static bool NotEquals(this float number, float otherNumber)
    {
        return Math.Abs(number - otherNumber) > EQUALS_TOLERANCE;
    }
}