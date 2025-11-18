using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class BiomeButton : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private BiomeConfig biome;      
    [SerializeField] private string gameplaySceneName = "Game";

    [Header("UI (opcional)")]
    [SerializeField] private TMP_Text labelTMP;
    [SerializeField] private Text labelUGUI;

    private void Start()
    {
        
        if (biome != null)
        {
            string name = string.IsNullOrEmpty(biome.displayName) ? biome.name : biome.displayName;
            if (labelTMP) labelTMP.text = name;
            if (labelUGUI) labelUGUI.text = name;
        }
    }

    public void OnBiomeSelected()
    {
        if (biome == null)
        {
            Debug.LogWarning("[BiomeButton] No hay BiomeConfig asignado.", this);
            return;
        }

        if (GameFacade.I != null)
        {
            GameFacade.I.SelectedMode = GameFacade.GameMode.Biome;
            GameFacade.I.SelectedBiome = biome;
            GameFacade.I.LoadScene(gameplaySceneName);
        }
        else
        {
           SceneManager.LoadScene(gameplaySceneName);
        }
    }
}
