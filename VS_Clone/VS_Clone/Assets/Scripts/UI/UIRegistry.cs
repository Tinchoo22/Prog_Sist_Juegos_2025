using UnityEngine;

public class UIRegistry : MonoBehaviour
{

    [SerializeField] private GameObject pausePanelRoot;
    [SerializeField] private GameObject gameOverPanelRoot;
    [SerializeField] private GameObject levelUpPanelRoot;
      
    [SerializeField] private bool verboseLogs = true;

    private void Start()
    {
        if (GameFacade.I == null)
        {
            if (verboseLogs) Debug.LogWarning("[UIRegistry] No hay GameFacade en escena. Asegurate de tener el _Facade (DontDestroyOnLoad).");
            return;
        }

        if (pausePanelRoot)
        {
            GameFacade.I.RegisterPausePanel(pausePanelRoot);
            if (verboseLogs) Debug.Log($"[UIRegistry] Registré PausePanel: {pausePanelRoot.name}");
        }
        else if (verboseLogs) Debug.LogWarning("[UIRegistry] Falta PausePanelRoot");

        if (gameOverPanelRoot)
        {
            GameFacade.I.RegisterGameOverPanel(gameOverPanelRoot);
            if (verboseLogs) Debug.Log($"[UIRegistry] Registré GameOverPanel: {gameOverPanelRoot.name}");
        }
        else if (verboseLogs) Debug.LogWarning("[UIRegistry] Falta GameOverPanelRoot");

        if (levelUpPanelRoot)
        {
            GameFacade.I.RegisterLevelUpPanel(levelUpPanelRoot);
            if (verboseLogs) Debug.Log($"[UIRegistry] Registré LevelUpPanel: {levelUpPanelRoot.name}");
        }
        else if (verboseLogs) Debug.LogWarning("[UIRegistry] Falta LevelUpPanelRoot");
    }
}

