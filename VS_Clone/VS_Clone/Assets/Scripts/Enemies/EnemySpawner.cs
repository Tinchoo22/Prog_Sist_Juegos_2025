using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
   
    [SerializeField] private EnemyFactory enemyFactory;   
    [SerializeField] private Transform player;            

    
    [SerializeField] private float baseInterval = 1f;
    [SerializeField] private float minInterval = 0.35f;
    [SerializeField] private int maxAlive = 60;
    [SerializeField] private float spawnRadius = 12f;

   
    private float t;         
    private float acc;      

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

    private void Update()
    {
        if (!enemyFactory || !player) return;

        var wave = BiomeManager.Instance?.CurrentWave;

        if (wave != null)
        {
           UpdateBiomeMode(wave);
        }
        else
        {
           UpdateClassicMode();
        }
    }

    private void UpdateClassicMode()
    {
        t += Time.deltaTime;

        float minutes = (GameSession.Instance?.RunTime ?? 0f) / 60f;
        float desiredInterval = Mathf.Clamp(baseInterval - minutes * 0.08f, minInterval, baseInterval);

        if (t >= desiredInterval)
        {
            t = 0f;
            TrySpawnClassic();
        }
    }

    private void TrySpawnClassic()
    {
        if (FindObjectsOfType<EnemyController>().Length >= maxAlive) return;
        SpawnOneClassic();
    }

    private void SpawnOneClassic()
    {
       
        float ang = Random.value * Mathf.PI * 2f;
        Vector2 pos = (Vector2)player.position + new Vector2(Mathf.Cos(ang), Mathf.Sin(ang)) * spawnRadius;

        var enemy = enemyFactory.SpawnRandom(pos);
        if (!enemy)
            Debug.LogError("[EnemySpawner] SpawnRandom devolvió null. Revisá EnemyFactory/Pool/Prefab.", this);
    }

    private void UpdateBiomeMode(WaveConfig wave)
    {
        float minutes = (GameSession.Instance?.RunTime ?? Time.time) / 60f;

       
        float totalSpm = 0f;
        List<EnemyArchetypeConfig> candidates = new List<EnemyArchetypeConfig>();

        foreach (var rule in wave.rules)
        {
            if (rule == null || rule.archetype == null) continue;
            if (minutes < rule.startMinute || minutes > rule.endMinute) continue;

            float spm = Mathf.Max(0f, rule.spawnsPerMinute.Evaluate(minutes));
            if (spm <= 0f) continue;

            totalSpm += spm;
                        
            int weight = Mathf.Clamp(Mathf.RoundToInt(spm), 1, 10);
            for (int i = 0; i < weight; i++)
                candidates.Add(rule.archetype);
        }

        if (candidates.Count == 0 || totalSpm <= 0.01f) return;

        
        int alive = CountAliveEnemies();
        if (alive >= wave.maxSimultaneous) return;

        float sps = totalSpm / 60f;
        acc += sps * Time.deltaTime;

        while (acc >= 1f && alive < wave.maxSimultaneous)
        {
            acc -= 1f;

           
            var arch = candidates[Random.Range(0, candidates.Count)];
                       
            Vector3 pos = PickSpawnPositionBiome(player.position, wave.minSpawnDistanceFromPlayer, wave.spawnRing);
                        
            enemyFactory.Spawn(arch, pos, minutes);

            alive++;
        }
    }

    private int CountAliveEnemies()
    {
        var enemies = FindObjectsOfType<EnemyController>();
        int alive = 0;
        foreach (var e in enemies) if (e.gameObject && e.gameObject.activeInHierarchy) alive++;
        return alive;
    }

    private Vector3 PickSpawnPositionBiome(Vector3 playerPos, float minDist, float ring)
    {
        
        for (int i = 0; i < 8; i++)
        {
            float ang = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(ang), Mathf.Sin(ang), 0f);
            Vector3 p = playerPos + dir * ring;
            if (Vector3.Distance(p, playerPos) >= minDist) return p;
        }
        return playerPos + (Vector3)Random.insideUnitCircle.normalized * ring;
    }
}
