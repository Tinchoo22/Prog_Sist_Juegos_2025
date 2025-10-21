using UnityEngine;
using UnityEngine.UI;

public class PlayerHPHud : MonoBehaviour
{
    public PlayerHealth player;
    public Slider hpBar;
    public Text hpText;

    void OnEnable()
    {
        EventBus.OnPlayerHPChanged += OnHPChanged;
    }
    void OnDisable()
    {
        EventBus.OnPlayerHPChanged -= OnHPChanged;
    }

    void Start()
    {
        if (!player) player = FindObjectOfType<PlayerHealth>();
        if (player)
        {
            if (hpBar) { hpBar.maxValue = player.maxHealth; hpBar.value = player.currentHealth; }
            if (hpText) hpText.text = $"{player.currentHealth} / {player.maxHealth}";
        }
        else
        {
            Debug.LogWarning("[PlayerHPHud] No se encontró PlayerHealth en escena.");
        }
    }

    void OnHPChanged(int current, int max)
    {
        if (hpBar) { hpBar.maxValue = max; hpBar.value = current; }
        if (hpText) hpText.text = $"{current} / {max}";
    }
}

