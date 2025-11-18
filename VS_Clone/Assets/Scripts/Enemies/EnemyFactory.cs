using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
   
    [SerializeField] private Pool enemyPool;                
    [SerializeField] private EnemyController enemyPrefab;    

    
    [SerializeField] private EnemyConfig[] configs;          

  
    [SerializeField] private bool verbose = false;

    private void Awake()
    {
        if (!enemyPool) Debug.LogError("[EnemyFactory] Falta enemyPool", this);
        if (!enemyPrefab) Debug.LogError("[EnemyFactory] Falta enemyPrefab", this);
        if (configs == null || configs.Length == 0)
            Debug.LogWarning("[EnemyFactory] 'configs' vacío (sólo afecta modo Direct)", this);
    }

    public EnemyController Spawn(EnemyConfig config, Vector2 pos)
    {
        if (!config)
        {
            Debug.LogError("[EnemyFactory] Config null");
            return null;
        }
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
            Debug.LogError("[EnemyFactory] El objeto devuelto por la Pool no tiene EnemyController");
            return null;
        }

        go.transform.position = pos;
        ctrl.Init(config);
        if (verbose) Debug.Log($"[EnemyFactory] Spawn → {config.name} en {pos}");
        return ctrl;
    }

    public EnemyController SpawnRandom(Vector2 pos)
    {
       
        if (GameFacade.I &&
            GameFacade.I.SelectedMode == GameFacade.GameMode.Biome &&
            GameFacade.I.SelectedBiome != null)
        {
            float rt = GameSession.Instance?.RunTime ?? 0f;
            var cfgBiome = GameFacade.I.SelectedBiome.GetEnemyForTime(rt);
            if (!cfgBiome)
            {
                Debug.LogError("[EnemyFactory] SelectedBiome devolvió NULL en SpawnRandom. Revisá BiomeConfig.", this);
                return null;
            }
            if (verbose) Debug.Log($"[EnemyFactory] SpawnRandom (forzado BIOME) → {cfgBiome.name}");
            return Spawn(cfgBiome, pos);
        }

     
        if (configs == null || configs.Length == 0)
        {
            Debug.LogWarning("[EnemyFactory] Sin configs para SpawnRandom (modo Direct).", this);
            return null;
        }

        var cfg = configs[Random.Range(0, configs.Length)];
        if (verbose) Debug.Log($"[EnemyFactory] SpawnRandom (DIRECT) → {cfg.name}");
        return Spawn(cfg, pos);
    }

    public void Despawn(EnemyController enemy)
    {
        if (!enemy) return;
        var pool = enemy.GetComponentInParent<Pool>();
        if (pool != null) pool.Return(enemy.gameObject);
        else enemy.gameObject.SetActive(false);
    }
}
