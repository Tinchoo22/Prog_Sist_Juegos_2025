using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeService : MonoBehaviour
{
    public static UpgradeService Instance { get; private set; }
        
    [SerializeField] private List<UpgradeConfig> allUpgrades = new List<UpgradeConfig>();
        
    [SerializeField] private int optionsPerLevel = 3;
        
    private readonly List<UpgradeConfig> offeredPool = new List<UpgradeConfig>();

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
      
    public List<UpgradeConfig> GetRandomOptions()
    {
        offeredPool.Clear();

        if (allUpgrades == null || allUpgrades.Count == 0)
            return offeredPool;

 
        var candidates = allUpgrades
            .Where(u => u != null && !IsMaxed(u))
            .ToList();

        if (candidates.Count == 0)
            return offeredPool;

        int count = Mathf.Min(optionsPerLevel, candidates.Count);
        for (int i = 0; i < count; i++)
        {
            int idx = Random.Range(0, candidates.Count);
            offeredPool.Add(candidates[idx]);
            candidates.RemoveAt(idx);
        }

        return offeredPool;
    }

    public List<UpgradeConfig> GetRandomOptions(int count)
    {
        var prev = optionsPerLevel;
        optionsPerLevel = (count > 0) ? count : prev;

        var result = GetRandomOptions();

        optionsPerLevel = prev;
        return result;
    }

    public void Apply(UpgradeConfig cfg) => ApplyUpgrade(cfg);

    public void ApplyUpgrade(UpgradeConfig cfg)
    {
        if (cfg == null || GameSession.Instance == null) return;
   
        int newLevel = GameSession.Instance.IncrementUpgrade(cfg.id, cfg.maxLevel);
               
        GameFacade.I?.PlaySfx("level_up");
    }
   
    public bool IsMaxed(UpgradeConfig cfg)
    {
        if (cfg == null || GameSession.Instance == null) return true;
        return GameSession.Instance.IsMaxed(cfg.id, cfg.maxLevel);
    }

    public int GetLevel(string upgradeId)
    {
        if (GameSession.Instance == null || string.IsNullOrEmpty(upgradeId)) return 0;
        return GameSession.Instance.GetUpgradeLevel(upgradeId);
    }

    public void ResetAll()
    {
        offeredPool.Clear();
    }
    public List<UpgradeConfig> AllUpgrades => allUpgrades;
}
