using UnityEngine;

public class PiercingBehavior : IProjectileBehavior
{
    private int remainingPierces;
    public PiercingBehavior(int pierces)
    {
        remainingPierces = Mathf.Max(0, pierces);
    }

    public void OnFire(Projectile p) { }

    public void Tick(Projectile p, float dt) { }

    public bool OnHit(Projectile p, Collider2D other, int damageApplied)
    {
        if (remainingPierces > 0)
        {
            remainingPierces--;
            return false; 
        }
        return true; 
    }
}

