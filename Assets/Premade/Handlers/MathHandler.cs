using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHandler
{
    public static Vector2 Vector3ToVector2(Vector3 vec)
    {
        return new Vector2(vec.x, vec.z);
    }
    public static Vector3 Vector2ToVector3(Vector2 vec, float yUp)
    {
        return new Vector3(vec.x, yUp ,vec.y);
    }
    public static Vector3 Vector2ToVector3(Vector2 vec)
    {
        return new Vector3(vec.x, 0, vec.y);
    }
    public static Vector3 ToVector3(this Vector2 vec)
    {
        return new Vector3(vec.x, 0, vec.y);
    }
    public static Vector2 ToVector2(this Vector3 vec)
    {
        return new Vector2(vec.x, vec.z);
    }
    public static Vector2 ToVector2_Y(this Vector3 vec)
    {
        return new Vector2(vec.x, vec.y);
    }
    public static Vector3 SetYZero(this Vector3 vec)
    {
        return new Vector3(vec.x, 0, vec.z);
    }
    public static Vector3 SetY(this Vector3 vec,float Y)
    {
        return new Vector3(vec.x, Y, vec.z);
    }
    public static Vector3 RotateVectorByAngle(Vector3 vec, float angleInDegrees)//angle in degrees anti clockwise
    {
        float angleInRadians = angleInDegrees * Mathf.PI / 180;
        
        return new Vector3(vec.x * Mathf.Cos(angleInRadians) - vec.z * Mathf.Sin(angleInRadians), vec.y, vec.z * Mathf.Cos(angleInRadians) + vec.x * Mathf.Sin(angleInRadians));
    }
}
