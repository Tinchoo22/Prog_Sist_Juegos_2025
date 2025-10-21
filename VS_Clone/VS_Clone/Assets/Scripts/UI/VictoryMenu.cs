using UnityEngine;

public class VictoryMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private void Start()
    {
        if (!panel) panel = gameObject;
        GameFacade.I?.RegisterVictoryPanel(panel);
        if (panel.activeSelf) panel.SetActive(false);
    }

    public void OnRetry()
    {
        GameFacade.I?.RestartNewRun();
    }

    public void OnMainMenu()
    {
        Time.timeScale = 1f;
        try { EventBus.ClearQueue(); } catch { }
        GameFacade.I?.ResetRunState();
        GameFacade.I?.QuitToMainMenu();
    }
}
