using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers 
{
    /// <summary>
    /// Determine if the transform needs to rotate right or left to face the target
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="target"></param>
    /// <returns>0 = facing target, 1 turn right, 2, turn left</returns>
    public static int RightOrLeft(Transform transform, Vector3 target)
    {
        float dir = Vector3.Dot(Vector3.Cross(transform.up, (transform.position - target).normalized), Vector3.back);

        if (dir > 0f)
        {
            return 1;
        }
        else if (dir < 0f)
        {
            return 2;
        }
        return 0;
    }
}
