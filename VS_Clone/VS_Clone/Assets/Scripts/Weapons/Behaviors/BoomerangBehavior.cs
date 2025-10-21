using UnityEngine;

public class BoomerangBehavior : IProjectileBehavior
{
    private readonly float distanceOut;
    private readonly float returnSpeedMultiplier;
    private Vector2 spawnPos;
    private bool returning = false;

    public BoomerangBehavior(float distanceOut, float returnSpeedMultiplier = 1.2f)
    {
        this.distanceOut = Mathf.Max(0.1f, distanceOut);
        this.returnSpeedMultiplier = Mathf.Max(0.1f, returnSpeedMultiplier);
    }

    public void OnFire(Projectile p)
    {
        spawnPos = p.transform.position;
        returning = false;
    }

    public void Tick(Projectile p, float dt)
    {
        if (!returning)
        {
            float dist = Vector2.Distance(spawnPos, p.transform.position);
            if (dist >= distanceOut)
            {
                var vel = p.GetVelocity();
                if (vel.sqrMagnitude > 0.0001f)
                {
                    Vector2 newVel = -vel.normalized * vel.magnitude * returnSpeedMultiplier;
                    p.SetVelocity(newVel);
                    returning = true;
                }
            }
        }
    }

    public bool OnHit(Projectile p, Collider2D other, int damageApplied)
    {
       return true;
    }
}
