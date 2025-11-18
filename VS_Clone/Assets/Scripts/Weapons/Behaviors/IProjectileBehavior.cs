using UnityEngine;
public interface IProjectileBehavior
{
    void OnFire(Projectile projectile);                         
    void Tick(Projectile projectile, float deltaTime);          
    bool OnHit(Projectile projectile, Collider2D other, int dmg); 
}


