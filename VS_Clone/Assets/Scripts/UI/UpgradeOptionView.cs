using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UpgradeOptionView : MonoBehaviour
{

    [SerializeField] private Button button;  
       
    [SerializeField] private Image icon;      
    [SerializeField] private Text titleText; 
    [SerializeField] private TMP_Text titleTMP; 
    [SerializeField] private Text levelText; 
    [SerializeField] private TMP_Text levelTMP; 
    [SerializeField] private Text descText; 
    [SerializeField] private TMP_Text descTMP;  
    [SerializeField] private Text previewText; 
    [SerializeField] private TMP_Text previewTMP; 

    private UpgradeConfig bound;
    private System.Action<UpgradeConfig> onClick;


    public void Bind(UpgradeConfig cfg, System.Action<UpgradeConfig> onClick)
    {
        bound = cfg;
        this.onClick = onClick;

        AutoWireIfNeeded();

        if (button == null)
        {
            Debug.LogError($"[UpgradeOptionView] No encontré Button en {name}");
            gameObject.SetActive(false);
            return;
        }

        if (icon && cfg.icon) icon.sprite = cfg.icon;

        SetText("Title", titleText, titleTMP, cfg.title);

        int cur = GameSession.Instance.GetUpgradeLevel(cfg.id);
        string lv = $"Lv {cur}/{cfg.maxLevel}";
        SetText("Level", levelText, levelTMP, lv);

        SetText("Desc", descText, descTMP, cfg.description);

        string preview = BuildPreviewForNextLevel(cfg, cur);
        SetText("Preview", previewText, previewTMP, preview);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick?.Invoke(bound));

        gameObject.SetActive(true);
    }


    private void AutoWireIfNeeded()
    {
        if (button == null) button = GetComponent<Button>() ?? GetComponentInChildren<Button>(true);
        if (icon == null) icon = FindChild<Image>("Icon");

        if (titleText == null && titleTMP == null) FindTextPair("Title", ref titleText, ref titleTMP);
        if (levelText == null && levelTMP == null) FindTextPair("Level", ref levelText, ref levelTMP);
        if (descText == null && descTMP == null) FindTextPair("Desc", ref descText, ref descTMP);
        if (previewText == null && previewTMP == null) FindTextPair("Preview", ref previewText, ref previewTMP);
    }

    private T FindChild<T>(string childName) where T : Component
    {
        var t = transform.Find(childName);
        return t ? t.GetComponent<T>() : GetComponentInChildren<T>(true);
    }

    private void FindTextPair(string childName, ref Text uiText, ref TMP_Text tmpText)
    {
        var t = transform.Find(childName);
        if (t)
        {
            uiText = t.GetComponent<Text>();
            tmpText = t.GetComponent<TMP_Text>();
        }
        else
        {
    
            foreach (var txt in GetComponentsInChildren<Text>(true))
                if (txt.name == childName) { uiText = txt; break; }
            if (tmpText == null)
                foreach (var tmp in GetComponentsInChildren<TMP_Text>(true))
                    if (tmp.name == childName) { tmpText = tmp; break; }
        }
    }

    private void SetText(string label, Text ui, TMP_Text tmp, string value)
    {
        if (ui != null) ui.text = value;
        if (tmp != null) tmp.text = value;
        if (ui == null && tmp == null)
            Debug.LogWarning($"[UpgradeOptionView] No encontré campo de texto '{label}' en {name}.");
    }


    private string BuildPreviewForNextLevel(UpgradeConfig cfg, int currentLevel)
    {
        if (currentLevel >= cfg.maxLevel) return "Maxeado";
        int next = currentLevel + 1;

        if (cfg is StatUpgradeConfig stat)
        {
            string statName = LocalizeStat(stat.statId);
            string delta = FormatDelta(stat.deltaPerLevel);
            return $"{delta} {statName} (al subir a Lv {next}/{cfg.maxLevel})";
        }

        if (cfg is AddWeaponUpgradeConfig aw)
        {
            if (next == 1)
            {
                string wName = aw.weaponConfig ? aw.weaponConfig.name : "Arma";
                return $"Desbloquea {wName}";
            }
            else
            {
                return $"Mejora del arma (Lv {next}/{cfg.maxLevel})";
            }
        }

        return $"Sube a Lv {next}/{cfg.maxLevel}";
    }

    private string FormatDelta(float v)
    {
        if (Mathf.Abs(v - Mathf.Round(v)) < 0.0001f) return $"+{Mathf.RoundToInt(v)}";
        return (v >= 0) ? $"+{v:0.##}" : $"{v:0.##}";
    }

    private string LocalizeStat(string statId)
    {
        switch (statId)
        {
            case "Damage": return "Daño";
            case "FireRate": return "Cadencia";
            case "MoveSpeed": return "Velocidad";
            default: return statId;
        }
    }

#if UNITY_EDITOR
    [SerializeField] private bool previewInEditor = false;
    [SerializeField] private UpgradeConfig previewUpgrade;

    private void OnValidate()
    {
        if (!previewInEditor || previewUpgrade == null) return;
  
        AutoWireIfNeeded();
        if (icon && previewUpgrade.icon) icon.sprite = previewUpgrade.icon;
        SetText("Title", titleText, titleTMP, previewUpgrade.title);
        SetText("Level", levelText, levelTMP, $"Lv 0/{previewUpgrade.maxLevel}");
        SetText("Desc", descText, descTMP, previewUpgrade.description);
        SetText("Preview", previewText, previewTMP, BuildPreviewForNextLevel(previewUpgrade, 0));
        UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
    }
#endif
}
