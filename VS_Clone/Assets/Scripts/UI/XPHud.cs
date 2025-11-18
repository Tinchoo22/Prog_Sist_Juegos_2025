using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPHud : MonoBehaviour
{
    [SerializeField] private Slider xpBar;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Awake()
    {
        if (xpBar == null)
            xpBar = GetComponentInChildren<Slider>(true);
        if (levelText == null)
            levelText = GetComponentInChildren<TextMeshProUGUI>(true);
    }

    private void Start()
    {
        if (GameSession.Instance == null)
        {
            Debug.LogError("? Falta GameSession en escena. Asegúrate de tener el prefab _Systems con GameSession activo.");
            enabled = false;
            return;
        }

        UpdateBar(GameSession.Instance.CurrentXP, GameSession.Instance.XPToNext);
        if (levelText != null)
            levelText.text = $"LvL {GameSession.Instance.PlayerLevel}";
    }

    private void OnEnable()
    {
        EventBus.OnXPChanged += UpdateBar;
        EventBus.OnLevelUp += OnLevelUp;
    }

    private void OnDisable()
    {
        EventBus.OnXPChanged -= UpdateBar;
        EventBus.OnLevelUp -= OnLevelUp;
    }

    private void UpdateBar(int current, int toNext)
    {
        if (xpBar == null) return;
        xpBar.maxValue = toNext;
        xpBar.value = current;
    }

    private void OnLevelUp(int level)
    {
        if (levelText != null)
            levelText.text = $"LvL {level}";
    }
}
