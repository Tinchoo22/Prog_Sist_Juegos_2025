using System.Collections.Generic;
using UnityEngine;

public class SpreadPattern : IFirePattern
{
    private readonly int count;
    private readonly float totalAngleDeg;
    public SpreadPattern(int count, float totalAngleDeg)
    {
        this.count = Mathf.Max(1, count);
        this.totalAngleDeg = Mathf.Max(0f, totalAngleDeg);
    }

    public IEnumerable<Vector2> GetDirections(Vector2 ownerPos, Transform target, Vector2 baseDir)
    {
        if (count == 1)
        {
            yield return baseDir;
            yield break;
        }

        float step = (count > 1) ? totalAngleDeg / (count - 1) : 0f;
        float half = totalAngleDeg * 0.5f;

        for (int i = 0; i < count; i++)
        {
            float angle = -half + step * i;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * baseDir;
            yield return dir.normalized;
        }
    }
}
