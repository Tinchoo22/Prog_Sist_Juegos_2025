using System.Collections.Generic;
using UnityEngine;

public class LevelUpMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private UpgradeOptionView[] optionViews;
    [SerializeField] private UpgradeService upgradeService;
    private bool isOpen;

    private void OnEnable()
    {
        EventBus.OnLevelUp += OpenWithOptions;
    }

    private void OnDisable()
    {
        EventBus.OnLevelUp -= OpenWithOptions;
    }

    private void OpenWithOptions(int level)
    {
        if (isOpen) return;

        if (!panel || optionViews == null || optionViews.Length == 0 || !upgradeService)
        {
            Debug.LogWarning("[LevelUpMenu] Faltan referencias o la escena está cambiando. Evento ignorado.");
            return;
        }

        if (GameSession.Instance == null || UpgradeService.Instance == null)
        {
            Debug.LogWarning("[LevelUpMenu] No hay sesión o servicio activos (probablemente cambio de escena).");
            return;
        }

        GameSession.Instance.SnapshotBeforeLevelUp();

        int count = optionViews.Length;
        List<UpgradeConfig> options = upgradeService.GetRandomOptions(count);

        for (int i = 0; i < optionViews.Length; i++)
        {
            if (i < options.Count)
            {
                optionViews[i].gameObject.SetActive(true);
                optionViews[i].Bind(options[i], OnPick);
            }
            else
            {
                optionViews[i].gameObject.SetActive(false);
            }
        }

        panel.SetActive(true);
        isOpen = true;
        Time.timeScale = 0f;
    }

    private void OnPick(UpgradeConfig cfg)
    {
        try
        {
            if (upgradeService && cfg != null)
                upgradeService.Apply(cfg);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }

        Close();
    }

    public void Close()
    {
        if (!isOpen) return;

        panel.SetActive(false);
        isOpen = false;
        Time.timeScale = 1f;
        GameSession.Instance?.ClearPendingSnapshot();
    }

    public void CancelSelection()
    {
        GameSession.Instance?.RestoreSnapshotIfNeeded();
        Close();
    }
}
