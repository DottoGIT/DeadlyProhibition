using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalUtils
{
    public static Quaternion LookAt(Vector2 origin, Vector2 targetPosition, float offset = 0)
    {
        float RotZ = Mathf.Rad2Deg * Mathf.Asin(((targetPosition.y - origin.y) / Mathf.Sqrt(Mathf.Pow(targetPosition.x - origin.x, 2) + Mathf.Pow(targetPosition.y - origin.y, 2))));
        if (targetPosition == origin)
        {
            return Quaternion.Euler(0, 0, 0);
        }
        else if (targetPosition.x > origin.x)
        {
            return Quaternion.Euler(0, 0, RotZ + offset);
        }
        else
        {
            return Quaternion.Euler(0, 0, -RotZ + offset + 180);
        }
    }
}
