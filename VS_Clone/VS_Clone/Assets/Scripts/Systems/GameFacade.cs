using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameFacade : MonoBehaviour
{
    public static GameFacade I { get; private set; }

    
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject levelUpPanel;
    [SerializeField] private GameObject victoryPanel;

    [SerializeField] private bool verboseLogs = true;

    private bool isPaused;

    private void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (I == this) SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isPaused = false;
        Time.timeScale = 1f;
        try { EventBus.ClearQueue(); } catch { }

        pauseMenuPanel = null;
        gameOverPanel = null;
        levelUpPanel = null;
        victoryPanel = null;

        isPaused = false;
        Time.timeScale = 1f;
        try { EventBus.ClearQueue(); } catch { }

        if (verboseLogs)
            Debug.Log($"[GameFacade] Cargada escena: {scene.name}");

        StartCoroutine(DeferredWire());
    }

    private IEnumerator DeferredWire()
    {
        yield return null;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (!IsGameplayScene()) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) Pause(true);
            else Pause(false);
        }
    }

    private bool IsGameplayScene()
    {
        string name = SceneManager.GetActiveScene().name;
        return name != "MainMenu";
    }
 
    private void EnsurePausePanel()
    {
        if (pauseMenuPanel) return;
        var ui = FindObjectOfType<PauseMenuUI>(true);
        if (ui) pauseMenuPanel = ui.gameObject;
        if (!pauseMenuPanel)
        {
            var go = GameObject.Find("Panel_Pause");
            if (go) pauseMenuPanel = go;
        }
    }

    private void EnsureGameOverPanel()
    {
        if (gameOverPanel) return;
        var ui = FindObjectOfType<GameOverMenu>(true);
        if (ui) gameOverPanel = ui.gameObject;
        if (!gameOverPanel)
        {
            var go = GameObject.Find("Panel_GameOver");
            if (go) gameOverPanel = go;
        }
    }

    private void EnsureLevelUpPanel()
    {
        if (levelUpPanel) return;
        var go = GameObject.Find("LevelUpPanel");
        if (go) levelUpPanel = go;
        var menu = FindObjectOfType<LevelUpMenu>(true);
        if (!levelUpPanel && menu) levelUpPanel = menu.gameObject;
    }

    private void EnsureVictoryPanel()
    {
        if (victoryPanel) return;
        var ui = FindObjectOfType<VictoryMenu>(true);
        if (ui) victoryPanel = ui.gameObject;
        if (!victoryPanel)
        {
            var go = GameObject.Find("VictoryPanel");
            if (go) victoryPanel = go;
        }
    }

    public void Pause(bool p)
    {
        if (!IsGameplayScene()) return;

        isPaused = p;
        if (p && !pauseMenuPanel) EnsurePausePanel();

        if (pauseMenuPanel)
        {
            var ui = pauseMenuPanel.GetComponent<PauseMenuUI>();
            if (!ui) ui = pauseMenuPanel.GetComponentInChildren<PauseMenuUI>(true);
            if (ui) ui.SetVisible(p, forceAlpha: true);
            else pauseMenuPanel.SetActive(p);
        }

        Time.timeScale = p ? 0f : 1f;
    }

    public void GameOver()
    {
        if (!IsGameplayScene()) return;

        if (!gameOverPanel) EnsureGameOverPanel();
        if (gameOverPanel) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowLevelUp(bool show)
    {
        if (!IsGameplayScene()) return;

        if (show && !levelUpPanel) EnsureLevelUpPanel();
        if (levelUpPanel) levelUpPanel.SetActive(show);
        Time.timeScale = show ? 0f : 1f;
    }

    public void Victory()
    {
        if (!IsGameplayScene()) return;

        if (!victoryPanel) EnsureVictoryPanel();
        if (victoryPanel) victoryPanel.SetActive(true);

     
        Time.timeScale = 0f;
    }
 
    public void LoadScene(string sceneName)
    {
        try { EventBus.ClearQueue(); } catch { }
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void RestartCurrent()
    {
        try { EventBus.ClearQueue(); } catch { }
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMainMenu()
    {
        try { EventBus.ClearQueue(); } catch { }
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetRunState()
    {
        if (GameSession.Instance != null)
        {
            GameSession.Instance.ResetCounters();
            GameSession.Instance.ResetRunTime();
        }

        var psm = FindObjectOfType<PlayerStateMachine>(true);
        if (psm != null) psm.HealToFull();

        foreach (var e in FindObjectsOfType<EnemyController>(true))
            if (e) e.gameObject.SetActive(false);

        foreach (var o in FindObjectsOfType<XPOrb>(true))
            if (o) Object.Destroy(o.gameObject);

        foreach (var pr in FindObjectsOfType<Projectile>(true))
        {
            if (!pr) continue;
            try { pr.Despawn(); } catch { pr.gameObject.SetActive(false); }
        }
    }

    public void RestartNewRun()
    {
        ResetRunState();
        RestartCurrent();
    }

    public void GoToMainMenuNewRun()
    {
        ResetRunState();
        QuitToMainMenu();
    }

    public void RegisterPausePanel(GameObject panel) => pauseMenuPanel = panel;
    public void RegisterGameOverPanel(GameObject panel) => gameOverPanel = panel;
    public void RegisterLevelUpPanel(GameObject panel) => levelUpPanel = panel;
    public void RegisterVictoryPanel(GameObject panel) => victoryPanel = panel;

    public void PlaySfx(string id) {  }
}
