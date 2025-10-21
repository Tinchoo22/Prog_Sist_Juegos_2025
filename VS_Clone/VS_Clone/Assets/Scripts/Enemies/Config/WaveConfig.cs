using UnityEngine;


public class WaveConfig : ScriptableObject
{
    [System.Serializable]
    public class SpawnRule
    {
        public EnemyArchetypeConfig archetype;
        [Tooltip("Desde qué minuto empieza a spawnear")]
        public float startMinute = 0f;
        [Tooltip("Hasta qué minuto aporta spawns (si fin < inicio, ignora fin)")]
        public float endMinute = 999f;

        [Tooltip("Spawns por minuto en función del tiempo (minutos). y=spawns/min")]
        public AnimationCurve spawnsPerMinute = AnimationCurve.Linear(0, 5, 10, 20);
    }

    public SpawnRule[] rules;

    public int maxSimultaneous = 60;
    public float minSpawnDistanceFromPlayer = 8f;
    public float spawnRing = 12f;              
}
