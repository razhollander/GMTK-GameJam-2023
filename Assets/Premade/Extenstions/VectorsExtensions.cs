using UnityEngine;

public static class VectorsExtensions
{
    public static Vector3 ToUnityVector3(this System.Numerics.Vector3 vector3)
    {
        return new Vector3(vector3.X, vector3.Y, vector3.Z);
    }

    public static System.Numerics.Vector3 ToNumericsVector3(this Vector3 vector3)
    {
        return new System.Numerics.Vector3(vector3.x, vector3.y, vector3.z);
    }

    public static float GetDistance(this Vector3[] path)
    {
        float pathDistance = 0;

        for (int i = 0; i < path.Length - 1; i++)
        {
            pathDistance += Vector3.Distance(path[i], path[i + 1]);
        }

        return pathDistance;
    }
}