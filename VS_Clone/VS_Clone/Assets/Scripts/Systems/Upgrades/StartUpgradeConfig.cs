using UnityEngine;


public class StatUpgradeConfig : UpgradeConfig
{
    [Header("Stat")]
    public string statId = "Damage";   
    public float deltaPerLevel = 1f;  

    public override void Apply(GameSession session, int newLevel)
    {
        if (session == null || session.Stats == null)
        {
            Debug.LogWarning("[StatUpgrade] Session/Stats nulos");
            return;
        }
                
        session.Stats.Add(statId, deltaPerLevel);
        Debug.Log($"[StatUpgrade] {statId} +{deltaPerLevel} (Lv {newLevel}/{maxLevel})");
    }
}

