using System;
using System.Numerics;

public static class MathUtils
{
    /// <summary>
    /// https://stackoverflow.com/questions/345187/math-mapping-numbers
    /// </summary>
    /// <param name="minValueOnRangeA"></param>
    /// <param name="maxValueOnRangeA"></param>
    /// <param name="minValueOnRangeB"></param>
    /// <param name="maxValueOnRangeB"></param>
    /// <param name="valueOnRangeA"></param>
    /// <returns></returns>
    public static float RemapBetweenRange(float minValueOnRangeA, float maxValueOnRangeA, float minValueOnRangeB,
        float maxValueOnRangeB, float valueOnRangeA)
    {
        var valueOnRangeB =
            (valueOnRangeA - minValueOnRangeA) / (maxValueOnRangeA - minValueOnRangeA) *
            (maxValueOnRangeB - minValueOnRangeB) + minValueOnRangeB;

        return valueOnRangeB;
    }

    public static bool IsNumberInRange(int numberToCheck, int start, int end)
    {
        return (numberToCheck >= start && numberToCheck <= end);
    }

    public static Vector2 CalculateLine(float x1, float x2, float y1, float y2, float t)
    {
        var denominator = (float) Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

        return new Vector2(x1, y1) + t * new Vector2((x2 - x1) / denominator, (y2 - y1) / denominator);
    }

    public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
    {
        if (val.CompareTo(min) < 0)
        {
            return min;
        }
        else if (val.CompareTo(max) > 0)
        {
            return max;
        }
        else
        {
            return val;
        }
    }
}