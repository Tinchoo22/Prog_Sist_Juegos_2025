using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private void Start()
    {
        
        if (!panel) panel = gameObject;

        GameFacade.I?.RegisterGameOverPanel(panel);

        if (panel.activeSelf) panel.SetActive(false);
    }

    private void OnEnable()
    {
        EventBus.OnPlayerDied += Show;
    }

    private void OnDisable()
    {
        EventBus.OnPlayerDied -= Show;
    }

    private void Show()
    {
        GameFacade.I?.GameOver();
        if (panel) panel.SetActive(true);
    }

    public void Retry()
    {
        GameFacade.I?.RestartNewRun();
    }

    public void QuitToMenu()
    {
       
        Time.timeScale = 1f;
        try { EventBus.ClearQueue(); } catch { }

        GameFacade.I?.ResetRunState();  
        GameFacade.I?.QuitToMainMenu();
    }
}
