
using System.Collections.Generic;
using UnityEngine;

public class MultiShotPattern : IFirePattern
{
    private readonly int count;

    public MultiShotPattern(int count)
    {
        this.count = Mathf.Max(1, count);
    }

    public IEnumerable<Vector2> GetDirections(Vector2 ownerPos, Transform target, Vector2 baseDir)
    {
        for (int i = 0; i < count; i++)
            yield return baseDir;
    }
}
