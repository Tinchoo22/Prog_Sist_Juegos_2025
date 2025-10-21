using UnityEngine;

public class CryptBiomeFactory : MonoBehaviour, IBiomeFactory
{
    [SerializeField] private WaveConfig waveConfig;
    [SerializeField] private EnemyArchetypeConfig[] catalog;

    public string BiomeId => "crypt";
    public WaveConfig GetWaveConfig() => waveConfig;
    public EnemyArchetypeConfig[] GetCatalog() => catalog;
}
