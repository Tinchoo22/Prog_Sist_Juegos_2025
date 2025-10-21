using UnityEngine;

public class BiomeManager : MonoBehaviour
{
    public static BiomeManager Instance { get; private set; }

    [Header("Fábricas disponibles")]
    [SerializeField] private MonoBehaviour[] factories;
    [Header("Bioma actual (índice)")]
    [SerializeField] private int selectedIndex = 0;

    private IBiomeFactory current;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Select(selectedIndex);
    }

    public void Select(int index)
    {
        if (factories == null || factories.Length == 0) { current = null; return; }
        index = Mathf.Clamp(index, 0, factories.Length - 1);
        current = factories[index] as IBiomeFactory;
        if (current == null)
            Debug.LogError("[BiomeManager] La referencia no implementa IBiomeFactory.");
    }

    public IBiomeFactory CurrentFactory => current;
    public WaveConfig CurrentWave => current?.GetWaveConfig();
    public EnemyArchetypeConfig[] CurrentCatalog => current?.GetCatalog();
}

