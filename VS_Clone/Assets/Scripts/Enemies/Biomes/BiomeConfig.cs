using UnityEngine;

[CreateAssetMenu(menuName = "Game/Biome", fileName = "NewBiome")]
public class BiomeConfig : ScriptableObject
{
    [Header("Identidad")]
    public string biomeId = "graveyard";
    public string displayName = "Graveyard";

    [Header("Listas de enemigos por etapa")]
    public EnemyConfig[] earlyEnemies; 
    public EnemyConfig[] midEnemies;   
    public EnemyConfig[] lateEnemies;  


    public EnemyConfig GetEnemyForTime(float runTimeSeconds)
    {
        float minutes = runTimeSeconds / 60f;
        EnemyConfig[] list = null;

        if (minutes < 2f) list = earlyEnemies;
        else if (minutes < 4f) list = midEnemies;
        else list = lateEnemies;

        if (list == null || list.Length == 0)
        {
         
            if (earlyEnemies != null && earlyEnemies.Length > 0) list = earlyEnemies;
            else if (midEnemies != null && midEnemies.Length > 0) list = midEnemies;
            else if (lateEnemies != null && lateEnemies.Length > 0) list = lateEnemies;
            else return null; 
        }

        int idx = Random.Range(0, list.Length);
        return list[idx];
    }
}
