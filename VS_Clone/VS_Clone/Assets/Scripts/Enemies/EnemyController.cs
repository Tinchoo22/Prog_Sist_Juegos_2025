using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
public class EnemyController : MonoBehaviour
{
  
    private EnemyConfig cfg;        
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform player;
    private int hp;
        
    private bool usingRuntime = false;
    private float runtimeSpeed = 0f;
    private int runtimeDamage = 0;
    private int runtimeXpDrop = 0;

    
    [SerializeField] private float contactDamageInterval = 0.6f;
    private float lastContactTime = -999f;

    [SerializeField] private bool logDamage = false;
    [SerializeField] private bool logContact = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

 
    public void Init(EnemyConfig config)
    {
        cfg = config;
        usingRuntime = false;

        if (sr && cfg && cfg.sprite) sr.sprite = cfg.sprite;
        hp = (cfg != null) ? cfg.maxHP : 1;

        if (!player)
        {
            var p = GameObject.FindWithTag("Player");
            if (p) player = p.transform;
        }

        if (rb) rb.velocity = Vector2.zero;
        lastContactTime = -999f;
        gameObject.SetActive(true);
    }


    public void Init(int hp, float moveSpeed, int contactDamage, int xpDrop)
    {
        cfg = null;                 
        usingRuntime = true;

        this.hp = Mathf.Max(1, hp);
        runtimeSpeed = Mathf.Max(0f, moveSpeed);
        runtimeDamage = Mathf.Max(0, contactDamage);
        runtimeXpDrop = Mathf.Max(0, xpDrop);

        if (!player)
        {
            var p = GameObject.FindWithTag("Player");
            if (p) player = p.transform;
        }

        if (rb) rb.velocity = Vector2.zero;
        lastContactTime = -999f;
        gameObject.SetActive(true);
    }


    private void FixedUpdate()
    {
        if (!player || !rb) return;

        float speed = GetSpeed();
        if (speed <= 0f) return;

        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rb.velocity = dir * speed;
    }

    public void TakeDamage(int dmg)
    {
        int dd = Mathf.Max(0, dmg);
        hp -= dd;
        if (logDamage) Debug.Log($"[EnemyController] {name} recibió {dd} → hp={hp}");
        if (hp <= 0) Die();
    }

    private void Die()
    {
        GameSession.Instance?.RegisterKill();
          
        int xp = GetXpDrop();
        if (xp > 0)
            XPOrb.Drop(transform.position, xp);
          
        var factory = FindObjectOfType<EnemyFactory>();
        if (factory != null) factory.Despawn(this);
        else gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (rb) rb.velocity = Vector2.zero;
    }


    private void OnCollisionEnter2D(Collision2D col) { TryContactDamage(col.collider, true); }
    private void OnCollisionStay2D(Collision2D col) { TryContactDamage(col.collider, false); }
    private void OnTriggerEnter2D(Collider2D col) { TryContactDamage(col, true); }
    private void OnTriggerStay2D(Collider2D col) { TryContactDamage(col, false); }

    private void TryContactDamage(Component other, bool enter)
    {
        int dmg = GetContactDamage();
        if (dmg <= 0) return;

        var psm = other.GetComponent<PlayerStateMachine>();
        if (!psm) return;

        if (enter)
        {
            if (logContact) Debug.Log($"[EnemyController] ENTER contacto → dmg {dmg}");
            psm.TakeDamage(dmg);
            lastContactTime = Time.time;
            return;
        }

        if (Time.time - lastContactTime >= contactDamageInterval)
        {
            if (logContact) Debug.Log($"[EnemyController] STAY contacto (tick) → dmg {dmg}");
            psm.TakeDamage(dmg);
            lastContactTime = Time.time;
        }
    }

   
    private float GetSpeed()
    {
        if (!usingRuntime && cfg != null) return cfg.speed;
        return runtimeSpeed;
    }

    private int GetContactDamage()
    {
        if (!usingRuntime && cfg != null) return cfg.damage;
        return runtimeDamage;
    }

    private int GetXpDrop()
    {
        if (!usingRuntime && cfg != null) return cfg.xpDrop;
        return runtimeXpDrop;
    }
}



