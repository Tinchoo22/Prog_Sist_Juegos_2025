using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private EnemyFactory enemyFactory;   
    [SerializeField] private Transform player;           

    [Header("Bioma (Abstract Factory)")]
    [SerializeField] private BiomeConfig biome;          

    [Header("Spawn Config")]
    [SerializeField] private float baseInterval = 1f;
    [SerializeField] private float minInterval = 0.35f;
    [SerializeField] private int maxAlive = 60;
    [SerializeField] private float spawnRadius = 12f;

    [Header("Debug")]
    [SerializeField] private bool verbose = true;

    private float t;
    private bool biomeModeActive;

    private void Awake()
    {
        if (!player)
        {
            var p = GameObject.FindWithTag("Player");
            if (p) player = p.transform;
        }

        if (!enemyFactory)
            Debug.LogError("[EnemySpawner] Falta EnemyFactory en el inspector", this);
    }

    private void Start()
    {
        var gf = GameFacade.I;
        if (gf && gf.SelectedMode == GameFacade.GameMode.Biome && gf.SelectedBiome != null)
        {
            biome = gf.SelectedBiome;
            biomeModeActive = true;
        }
        else
        {
            biomeModeActive = false;
        }

        if (verbose)
        {
            Debug.Log($"[EnemySpawner] Modo bioma activo = {biomeModeActive} | Biome = {(biome ? biome.displayName : "NULL")}", this);
            var allSpawners = FindObjectsOfType<EnemySpawner>();
            if (allSpawners.Length > 1)
                Debug.LogWarning($"[EnemySpawner] Hay {allSpawners.Length} spawners en escena. Esto puede mezclar enemigos.", this);
        }
    }

    private void Update()
    {
        if (!enemyFactory || !player) return;

        t += Time.deltaTime;

        float minutes = (GameSession.Instance?.RunTime ?? 0f) / 60f;
        float desiredInterval = Mathf.Clamp(baseInterval - minutes * 0.08f, minInterval, baseInterval);

        if (t >= desiredInterval)
        {
            t = 0f;
            TrySpawn();
        }
    }

    private void TrySpawn()
    {
        if (FindObjectsOfType<EnemyController>().Length >= maxAlive) return;
        SpawnOne();
    }

    private void SpawnOne()
    {
        float ang = Random.value * Mathf.PI * 2f;
        Vector2 pos = (Vector2)player.position + new Vector2(Mathf.Cos(ang), Mathf.Sin(ang)) * spawnRadius;

        EnemyController enemy = null;

        if (biomeModeActive && biome != null)
        {
            float rt = GameSession.Instance?.RunTime ?? 0f;
            var cfg = biome.GetEnemyForTime(rt);
            if (!cfg)
            {
                Debug.LogError("[EnemySpawner] Biome.GetEnemyForTime devolvió NULL. Revisá listas early/mid/late.", this);
                return;
            }
            if (verbose) Debug.Log($"[EnemySpawner] SPAWN (BIOME) {cfg.name} @t={rt:0.0}s", this);
            enemy = enemyFactory.Spawn(cfg, pos);
        }
        else
        {
            if (verbose) Debug.Log("[EnemySpawner] SPAWN (RANDOM global)", this);
            enemy = enemyFactory.SpawnRandom(pos);
        }

        if (!enemy)
            Debug.LogError("[EnemySpawner] Spawn devolvió null. Revisá Factory/Pool/Prefabs/Configs.", this);
    }
}

