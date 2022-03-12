using System.Collections;
using UnityEngine;

public static class VectorHelper
{
    public static Vector3 CorrectToCartesianXZ(Vector3 direction)
    {
        float absX = Mathf.Abs(direction.x);
        float absZ = Mathf.Abs(direction.z);

        if (absX >= absZ)
        {
            if (direction.x > 0)
            {
                return Vector3.right;
            } else
            {
                return Vector3.left;
            }
        } else
        {
            if (direction.z > 0)
            {
                return Vector3.forward;
            } else
            {
                return Vector3.back;
            }
        }
    }
}