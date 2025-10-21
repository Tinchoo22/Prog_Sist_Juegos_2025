using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private Selectable defaultButton;
    [SerializeField] private bool forceCanvasGroup = true;

    private CanvasGroup cg;
    private void Awake()
    {
        if (!panelRoot) panelRoot = gameObject;
        cg = panelRoot.GetComponent<CanvasGroup>();
        if (forceCanvasGroup && !cg) cg = panelRoot.AddComponent<CanvasGroup>();
    }

    private void Start()
    { 
        GameFacade.I?.RegisterPausePanel(panelRoot);
        SetVisible(false, forceAlpha: true);
    }

    private void OnEnable()
    {
        if (defaultButton)
            EventSystem.current?.SetSelectedGameObject(defaultButton.gameObject);
        else
            EventSystem.current?.SetSelectedGameObject(null);
    }

    private void OnDisable()
    {
        EventSystem.current?.SetSelectedGameObject(null);
    }

    public void OnResume()
    {
        GameFacade.I?.Pause(false);
        EventSystem.current?.SetSelectedGameObject(null);
    }

    public void OnRestart() => GameFacade.I?.RestartNewRun();
    public void OnMainMenu() => GameFacade.I?.GoToMainMenuNewRun();

     public void SetVisible(bool v, bool forceAlpha = false)
    {
        if (!panelRoot) return;

        panelRoot.SetActive(true);

        if (cg && forceAlpha)
        {
            cg.alpha = v ? 1f : 0f;
            cg.interactable = v;
            cg.blocksRaycasts = v;
        }
        else
        {
          if (v) panelRoot.SetActive(true);
          else panelRoot.SetActive(false);
        }
    }
}

