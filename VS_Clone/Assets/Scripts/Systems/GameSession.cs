using UnityEngine;
using System;
using System.Collections.Generic;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    [SerializeField] private int level = 1;
    [SerializeField] private int currentXP = 0;

    [SerializeField] private int baseXpToNext = 10;

    [SerializeField] private float xpGrowth = 1.35f;

    [SerializeField] private int kills = 0;

    [SerializeField] private float runTime = 0f; 

    public PlayerStats Stats = new PlayerStats(); 

    
    public object PendingSnapshot { get; set; }

  
    public Action<int> OnLevelUp;                      
    public Action<int, int> OnXPChanged;               
    public Action<int> OnKillsChanged;                
    public Action<float> OnRunTimeChanged;            

    public int Kills => kills;
    public int KillCount => kills;         
    public int Level => level;
    public int PlayerLevel => level;       
    public int CurrentXP => currentXP;
    public int XPToNext => Mathf.Max(1, Mathf.RoundToInt(baseXpToNext * Mathf.Pow(xpGrowth, level - 1)));
    public float RunTime => runTime;
 
    private readonly Dictionary<string, int> upgradeLevels = new Dictionary<string, int>();

  
    public int GetUpgradeLevel(string id)
    {
        if (string.IsNullOrEmpty(id)) return 0;
        return upgradeLevels.TryGetValue(id, out var lvl) ? lvl : 0;
    }


    public bool IsMaxed(string id, int maxLevel)
    {
        if (string.IsNullOrEmpty(id)) return true;
        if (maxLevel <= 0) return true;
        return GetUpgradeLevel(id) >= maxLevel;
    }

  
    public int IncrementUpgrade(string id, int maxLevel)
    {
        if (string.IsNullOrEmpty(id)) return 0;
        int curr = GetUpgradeLevel(id);
        int next = Mathf.Min(curr + 1, Mathf.Max(1, maxLevel));
        upgradeLevels[id] = next;
        return next;
    }


    public void ResetUpgrades()
    {
        upgradeLevels.Clear();
    }
  
    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (Stats == null) Stats = new PlayerStats();
    }

    private void Update()
    {
        runTime += Time.deltaTime;
        OnRunTimeChanged?.Invoke(runTime);
    }


    public void RegisterKill()
    {
        kills++;
        OnKillsChanged?.Invoke(kills);
     
        try { EventBus.RaiseEnemyKilled(kills); } catch { }
    }

    public void AddXP(int amount)
    {
        if (amount <= 0) return;

        currentXP += amount;
               
        OnXPChanged?.Invoke(currentXP, XPToNext);
        try { EventBus.RaiseXPChanged(currentXP, XPToNext); } catch { }

      
        while (currentXP >= XPToNext)
        {
            currentXP -= XPToNext;
            level++;

            OnLevelUp?.Invoke(level);
            try { EventBus.RaiseLevelUp(level); } catch { }
              
            GameFacade.I?.ShowLevelUp(true);

            OnXPChanged?.Invoke(currentXP, XPToNext);
            try { EventBus.RaiseXPChanged(currentXP, XPToNext); } catch { }
        }
    }


    public void ResetCounters()
    {
        kills = 0;
        currentXP = 0;
        level = 1;
        PendingSnapshot = null;

     
        ResetUpgrades();

        OnKillsChanged?.Invoke(kills);
        OnXPChanged?.Invoke(currentXP, XPToNext);
        try
        {
            EventBus.RaiseEnemyKilled(kills);
            EventBus.RaiseXPChanged(currentXP, XPToNext);
        }
        catch { }
    }

   
    public void ResetRunTime()
    {
        runTime = 0f;
        OnRunTimeChanged?.Invoke(runTime);
    }

   
    private bool hasSnapshot = false;
    private int snapLevel = 0;
    private int snapXP = 0;

    public void SnapshotBeforeLevelUp()
    {
        snapLevel = level;
        snapXP = currentXP;
        hasSnapshot = true;
        PendingSnapshot = true; 
    }

    public void ClearPendingSnapshot()
    {
        hasSnapshot = false;
        PendingSnapshot = null;
    }

    public void RestoreSnapshotIfNeeded()
    {
        if (!hasSnapshot) return;
        level = snapLevel;
        currentXP = snapXP;
        hasSnapshot = false;
        PendingSnapshot = null;
            
        OnXPChanged?.Invoke(currentXP, XPToNext);
        try { EventBus.RaiseXPChanged(currentXP, XPToNext); } catch { }
    }
}
