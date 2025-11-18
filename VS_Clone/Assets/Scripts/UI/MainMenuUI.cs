using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
  
    [SerializeField] private GameObject mainMenuPanel;  
    [SerializeField] private GameObject playModePanel;  
    [SerializeField] private GameObject biomePanel;     
    [SerializeField] private GameObject optionsPanel; 


    [SerializeField] private string gameplaySceneName = "Game";

    private void Start()
    {
        ShowMainMenu();
    }
        
    private void ShowMainMenu()
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(true);
        if (playModePanel) playModePanel.SetActive(false);
        if (biomePanel) biomePanel.SetActive(false);
        if (optionsPanel) optionsPanel.SetActive(false);
    }

    private void ShowPlayModes()
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        if (playModePanel) playModePanel.SetActive(true);
        if (biomePanel) biomePanel.SetActive(false);
        if (optionsPanel) optionsPanel.SetActive(false);
    }

    private void ShowBiomes()
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        if (playModePanel) playModePanel.SetActive(false);
        if (biomePanel) biomePanel.SetActive(true);
        if (optionsPanel) optionsPanel.SetActive(false);
    }

    private void ShowOptions()
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        if (playModePanel) playModePanel.SetActive(false);
        if (biomePanel) biomePanel.SetActive(false);
        if (optionsPanel) optionsPanel.SetActive(true);
    }

    public void OnPlay()
    {
        ShowPlayModes();
    }

    public void OnOptions()
    {
        ShowOptions();
    }

    public void OnExit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

      public void OnPlayDirect()
    {
        if (GameFacade.I != null)
        {
            GameFacade.I.SelectedMode = GameFacade.GameMode.Direct;
            GameFacade.I.SelectedBiome = null;
            GameFacade.I.LoadScene(gameplaySceneName);
        }
        else
        {
            SceneManager.LoadScene(gameplaySceneName);
        }
    }
    public void OnPlayBiomes()
    {
        Debug.Log("[MainMenuUI] OnPlayBiomes llamado");
        ShowBiomes();
        
    }

    public void OnBackFromPlayModes()
    {
        Debug.Log("[MainMenuUI] OnBackFromPlayModes llamado");
        ShowMainMenu();
    }

    public void OnBackFromBiomes()
    {
        ShowPlayModes();
    }
    public void OnBackFromOptions()
    {
        ShowMainMenu();
    }
}


