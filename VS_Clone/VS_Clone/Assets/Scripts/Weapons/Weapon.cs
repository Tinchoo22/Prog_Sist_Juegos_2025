using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
  
    [SerializeField] private WeaponConfig config;
        
    [SerializeField] private Transform owner;       
    [SerializeField] private Pool projectilePool;   

    private float cooldown; 
    private List<IFirePattern> patterns = new List<IFirePattern>();
    private BurstPattern burst; 

    private void Awake()
    {
        
        if (owner == null)
        {
            var p = GameObject.FindWithTag("Player");
            if (p != null) owner = p.transform;
        }

        if (config != null) BuildPatternsFromConfig();
    }

    public void Init(Transform ownerTransform, WeaponConfig cfg, Pool projPool /*, ITargetingStrategy strategy = null*/)
    {
        SetOwner(ownerTransform);
        SetConfig(cfg);
        SetProjectilePool(projPool);
        BuildPatternsFromConfig();
        cooldown = 0f;
    }

    public void SetOwner(Transform t) => owner = t;
    public void SetConfig(WeaponConfig cfg) { config = cfg; BuildPatternsFromConfig(); }
    public void SetProjectilePool(Pool p) => projectilePool = p;
    public WeaponConfig GetConfig() => config;

    private void BuildPatternsFromConfig()
    {
        patterns.Clear();
        burst = null;

        if (config == null) return;

       
        if (config.useMultiShot)
            patterns.Add(new MultiShotPattern(config.multiShotCount));

        
        if (config.useSpread)
            patterns.Add(new SpreadPattern(config.spreadCount, config.spreadAngleDeg));

        
        if (config.useBurst)
        {
            burst = new BurstPattern(config.burstShots, config.burstInterval);
            patterns.Add(burst);
        }
    }

    private void Update()
    {
        if (config == null || owner == null || projectilePool == null)
            return;

        float dt = Time.deltaTime;
        cooldown -= dt;
      
        if (burst != null && burst.IsBurstActive)
        {
            if (burst.TickBurst(dt))
                TryShoot(inBurst: true);
        }

        if (cooldown <= 0f)
        {
            TryShoot(inBurst: false);

            float finalRate = Mathf.Max(0.1f, GetFinalFireRate());
            cooldown = 1f / finalRate;
          
            if (burst != null)
                burst.StartBurst();
        }
    }

    private void TryShoot(bool inBurst)
    {
      
        Transform target = FindNearestEnemy(owner.position, config.fireRange);
        if (target == null) return;

        Vector2 baseDir = (target.position - owner.position).normalized;
        int finalDamage = GetFinalDamage();

        IEnumerable<Vector2> dirs = new Vector2[] { baseDir };
        foreach (var p in patterns)
        {
            if (p is BurstPattern) continue; 
            List<Vector2> next = new List<Vector2>();
            foreach (var d in dirs)
                next.AddRange(p.GetDirections(owner.position, target, d));
            dirs = next;
        }
              
        int extra = Mathf.Max(1, config.projectilesPerShot);
  
        foreach (var dir in dirs)
        {
            for (int i = 0; i < extra; i++)
            {
                GameObject go = projectilePool.Get();
                Projectile proj = go.GetComponent<Projectile>();
                if (proj == null)
                {
                    Debug.LogError("[Weapon] El objeto del pool no tiene Projectile.");
                    return;
                }

                proj.ClearBehaviors();

                if (config.hasPiercing && config.pierces > 0)
                    proj.AddBehavior(new PiercingBehavior(config.pierces));

                if (config.hasBoomerang)
                    proj.AddBehavior(new BoomerangBehavior(config.boomerangOutDistance, config.boomerangReturnSpeedMult));

                if (config.hasHoming)
                    proj.AddBehavior(new HomingBehavior(config.homingTurnRateDegPerSec, config.homingSeekRadius));

                proj.Fire(owner.position, dir, config.projectileSpeed, finalDamage);
            }
        }
    }

    private float GetFinalFireRate()
    {
        float bonus = GameSession.Instance ? GameSession.Instance.Stats.Get("FireRate") : 0f;
        return Mathf.Max(0.1f, config.fireRate + bonus);
    }

    private int GetFinalDamage()
    {
        float bonus = GameSession.Instance ? GameSession.Instance.Stats.Get("Damage") : 0f;
        int finalDamage = Mathf.RoundToInt(config.damage + bonus);
        return Mathf.Max(0, finalDamage);
    }

    private Transform FindNearestEnemy(Vector2 from, float maxRange)
    {
        float maxSqr = maxRange * maxRange;
        float best = float.MaxValue;
        Transform bestT = null;

        var enemiesByTag = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var e in enemiesByTag)
        {
            if (!e || !e.activeInHierarchy) continue;
            float d2 = ((Vector2)e.transform.position - from).sqrMagnitude;
            if (d2 < best && d2 <= maxSqr)
            {
                best = d2;
                bestT = e.transform;
            }
        }

        if (bestT == null)
        {
            var all = GameObject.FindObjectsOfType<EnemyController>();
            foreach (var ec in all)
            {
                if (!ec || !ec.gameObject.activeInHierarchy) continue;
                float d2 = ((Vector2)ec.transform.position - from).sqrMagnitude;
                if (d2 < best && d2 <= maxSqr)
                {
                    best = d2;
                    bestT = ec.transform;
                }
            }
        }

        return bestT;
    }
}
