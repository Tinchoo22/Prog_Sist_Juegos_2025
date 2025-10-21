using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject panelOptions;
    [SerializeField] private GameObject defaultSelectable; 

    [SerializeField] private string gameplaySceneName = "Game";

    private void Awake()
    {
        
        Time.timeScale = 1f;
        try { EventBus.ClearQueue(); } catch { }

        
        GameFacade.I?.Pause(false);
                
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
                
        if (panelOptions) panelOptions.SetActive(false);
                
        EventSystem.current?.SetSelectedGameObject(null);
        if (defaultSelectable)
            EventSystem.current?.SetSelectedGameObject(defaultSelectable);
    }

     public void OnPlay()
    {
      
        Time.timeScale = 1f;
        try { EventBus.ClearQueue(); } catch { }
              
        if (GameFacade.I) GameFacade.I.LoadScene(gameplaySceneName);
        else SceneManager.LoadScene(gameplaySceneName);
    }

    public void OnOptions()
    {
        if (panelOptions) panelOptions.SetActive(true);
        EventSystem.current?.SetSelectedGameObject(null);
    }

    public void OnBackFromOptions()
    {
        if (panelOptions) panelOptions.SetActive(false);
        EventSystem.current?.SetSelectedGameObject(defaultSelectable ? defaultSelectable : null);
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

