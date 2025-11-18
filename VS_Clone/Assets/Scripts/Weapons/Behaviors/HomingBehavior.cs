
using UnityEngine;

public class HomingBehavior : IProjectileBehavior
{
    private readonly float turnRateDegPerSec;
    private readonly float seekRadius;
    private Transform target;

    public HomingBehavior(float turnRateDegPerSec, float seekRadius)
    {
        this.turnRateDegPerSec = Mathf.Max(1f, turnRateDegPerSec);
        this.seekRadius = Mathf.Max(0.5f, seekRadius);
    }

    public void OnFire(Projectile p)
    {
        target = FindNearestEnemy(p.transform.position);
    }

    public void Tick(Projectile p, float dt)
    {
        if (target == null || !target.gameObject.activeInHierarchy)
            target = FindNearestEnemy(p.transform.position);
        if (target == null) return;

        Vector2 desired = ((Vector2)target.position - (Vector2)p.transform.position).normalized;
        Vector2 current = p.GetVelocity().normalized;

        float maxDeg = turnRateDegPerSec * dt;
        float angle = Vector2.SignedAngle(current, desired);
        float clamp = Mathf.Clamp(angle, -maxDeg, maxDeg);

        Vector2 newDir = Quaternion.Euler(0, 0, clamp) * current;
        float speed = p.GetVelocity().magnitude;
        p.SetVelocity(newDir.normalized * speed);
    }

    public bool OnHit(Projectile p, Collider2D other, int damageApplied)
    {
       return true;
    }

    private Transform FindNearestEnemy(Vector2 from)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(from, seekRadius, ~0);
        float best = float.MaxValue;
        Transform bestT = null;
        foreach (var h in hits)
        {
            if (!h || !h.gameObject.activeInHierarchy) continue;
            if (h.CompareTag("Enemy") || h.GetComponent<EnemyController>())
            {
                float d = (h.transform.position - (Vector3)from).sqrMagnitude;
                if (d < best) { best = d; bestT = h.transform; }
            }
        }
        return bestT;
    }
}

