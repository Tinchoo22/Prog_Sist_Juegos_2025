using UnityEngine;

public class ForestBiomeFactory : MonoBehaviour, IBiomeFactory
{
    [SerializeField] private WaveConfig waveConfig;
    [SerializeField] private EnemyArchetypeConfig[] catalog;

    public string BiomeId => "forest";
    public WaveConfig GetWaveConfig() => waveConfig;
    public EnemyArchetypeConfig[] GetCatalog() => catalog;
}
