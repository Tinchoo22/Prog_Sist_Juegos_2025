using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
  
    public int damage;
    public float speed;
    public Vector2 direction;

    [SerializeField] private float lifeTime = 5f;
    private float life;
    private Rigidbody2D rb;

    private readonly List<IProjectileBehavior> behaviors = new List<IProjectileBehavior>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Fire(Vector2 startPos, Vector2 dir, float spd, int dmg)
    {
        transform.position = startPos;
        direction = dir.normalized;
        speed = spd;
        damage = dmg;
        life = lifeTime;

        if (rb) rb.velocity = direction * speed;

        for (int i = 0; i < behaviors.Count; i++)
            behaviors[i].OnFire(this);

        gameObject.SetActive(true);
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        for (int i = 0; i < behaviors.Count; i++)
            behaviors[i].Tick(this, dt);

        life -= dt;
        if (life <= 0f) Despawn();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);

            bool shouldConsume = true;
  
            for (int i = 0; i < behaviors.Count; i++)
            {
                if (!behaviors[i].OnHit(this, other, damage))
                    shouldConsume = false;
            }

            if (shouldConsume) Despawn();
        }
        else
        {
           // Despawn();
        }
    }

    public void ClearBehaviors() => behaviors.Clear();
    public void AddBehavior(IProjectileBehavior b) { if (b != null) behaviors.Add(b); }

    public void SetVelocity(Vector2 vel) { if (rb) rb.velocity = vel; }
    public Vector2 GetVelocity() => rb ? rb.velocity : direction * speed;

    public void Despawn()
    {
        gameObject.SetActive(false);

    }
}
