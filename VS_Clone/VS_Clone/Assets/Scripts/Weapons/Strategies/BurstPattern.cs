using System.Collections.Generic;
using UnityEngine;

public class BurstPattern : IFirePattern
{
    private readonly int shotsPerBurst;
    private readonly float shotInterval;
    private float timer;
    private int remaining;

    public BurstPattern(int shotsPerBurst, float shotInterval)
    {
        this.shotsPerBurst = Mathf.Max(1, shotsPerBurst);
        this.shotInterval = Mathf.Max(0.01f, shotInterval);
        remaining = 0;
        timer = 0f;
    }
    public IEnumerable<Vector2> GetDirections(Vector2 ownerPos, Transform target, Vector2 baseDir)
    {
        yield return baseDir;
    }

    public void StartBurst()
    {
        remaining = shotsPerBurst;
        timer = 0f;
    }

    public bool TickBurst(float dt)
    {
        if (remaining <= 0) return false;
        timer -= dt;
        if (timer <= 0f)
        {
            remaining--;
            timer = (remaining > 0) ? shotInterval : 0f;
            return true;
        }
        return false;
    }

    public bool IsBurstActive => remaining > 0;
}
