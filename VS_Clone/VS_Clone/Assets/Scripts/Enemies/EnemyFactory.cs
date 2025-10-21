using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
   
    [SerializeField] private Pool enemyPool;                 
    [SerializeField] private EnemyController enemyPrefab;  

  
    [SerializeField] private EnemyConfig[] configs;       

    private void Awake()
    {
        if (!enemyPool) Debug.LogError("[EnemyFactory] Falta enemyPool", this);
        if (!enemyPrefab) Debug.LogError("[EnemyFactory] Falta enemyPrefab", this);
        if (configs == null || configs.Length == 0) Debug.LogError("[EnemyFactory] Sin EnemyConfig en 'configs'", this);
    }

  
    public EnemyController Spawn(EnemyConfig config, Vector2 pos)
    {
        if (!config) { Debug.LogError("[EnemyFactory] Config null"); return null; }
        if (!enemyPool) return null;

        GameObject go = enemyPool.Get();
        if (!go)
        {
            Debug.LogError("[EnemyFactory] Pool.Get() devolvió null");
            return null;
        }

        var ctrl = go.GetComponent<EnemyController>();
        if (!ctrl)
        {
            Debug.LogError("[EnemyFactory] El objeto de Pool no tiene EnemyController");
            return null;
        }

        go.transform.position = pos;
        ctrl.Init(config);
        return ctrl;
    }

  
    public EnemyController SpawnRandom(Vector2 pos)
    {
        if (configs == null || configs.Length == 0) return null;
        var cfg = configs[Random.Range(0, configs.Length)];
        return Spawn(cfg, pos);
    }

 
    public void Despawn(EnemyController enemy)
    {
        if (!enemy) return;
        var pool = enemy.GetComponentInParent<Pool>();
        if (pool != null)
        {
          pool.Return(enemy.gameObject);
        }
        else
        {
            enemy.gameObject.SetActive(false);
        }
    }

   
    public EnemyController Spawn(EnemyArchetypeConfig arch, Vector2 pos, float minutes)
    {
        if (arch == null)
        {
            Debug.LogError("[EnemyFactory] Arquetipo nulo");
            return null;
        }

        GameObject go = null;

       
        bool canUsePool = (enemyPool != null) &&
                          (arch.prefab == null || arch.prefab == enemyPrefab.gameObject);

        if (canUsePool)
        {
            go = enemyPool.Get();
            if (!go)
            {
                Debug.LogError("[EnemyFactory] Pool.Get() devolvió null");
                return null;
            }
            go.transform.position = pos;
        }
        else
        {
           
            var prefabToUse = arch.prefab != null ? arch.prefab : enemyPrefab.gameObject;
            go = Instantiate(prefabToUse, pos, Quaternion.identity);
        }

        var ctrl = go.GetComponent<EnemyController>();
        if (!ctrl)
        {
            Debug.LogError("[EnemyFactory] El objeto creado no tiene EnemyController");
            return null;
        }

       
        int hp = arch.baseHP;
        float spd = arch.moveSpeed;
        arch.ApplyScaling(minutes, ref hp, ref spd);

     
        ctrl.Init(hp, spd, arch.contactDamage, arch.xpDrop);

       
        if (arch.overrideSprite)
        {
            var sr = go.GetComponentInChildren<SpriteRenderer>();
            if (sr) sr.sprite = arch.overrideSprite;
        }

        return ctrl;
    }
}
