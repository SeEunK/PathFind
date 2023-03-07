using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GFunc
{
    public static Vector2Int Abs(Vector2Int target)
    {
        target.x = Mathf.Abs(target.x);
        target.y = Mathf.Abs(target.y);
        return target;
    }
}
