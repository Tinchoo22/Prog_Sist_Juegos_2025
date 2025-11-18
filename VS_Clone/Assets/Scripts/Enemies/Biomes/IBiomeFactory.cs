using UnityEngine;

public interface IBiomeFactory
{
    string BiomeId { get; }
    WaveConfig GetWaveConfig();
    EnemyArchetypeConfig[] GetCatalog();
}

