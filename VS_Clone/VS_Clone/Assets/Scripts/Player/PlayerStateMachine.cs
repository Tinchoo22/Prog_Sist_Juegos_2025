using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerStateMachine : MonoBehaviour
{
    
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int currentHP = 100;

    
    [SerializeField] private PlayerHealth health;
    [SerializeField] private bool logDamage = false;

    private bool useLocalHP = false;

    private void Awake()
    {
        if (!health) health = GetComponent<PlayerHealth>();

        if (health)
        {
            health.currentHealth = Mathf.Clamp(health.currentHealth, 0, Mathf.Max(1, health.maxHealth));
            currentHP = health.currentHealth;
            maxHP = health.maxHealth;
            useLocalHP = false;
        }
        else
        {
            maxHP = Mathf.Max(1, maxHP);
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);
            useLocalHP = true;
        }

        RefreshHud();
    }

    public int CurrentHP => useLocalHP ? currentHP : (health ? health.currentHealth : currentHP);
    public int MaxHP => useLocalHP ? maxHP : (health ? health.maxHealth : maxHP);

    public void TakeDamage(int dmg)
    {
        if (dmg <= 0) return;

        if (useLocalHP)
        {
            currentHP = Mathf.Max(0, currentHP - dmg);
            if (logDamage) Debug.Log($"[Player] daño {dmg} → hp={currentHP}/{maxHP}");
            RefreshHud();
            if (currentHP <= 0) Die();
        }
        else if (health)
        {
            health.currentHealth = Mathf.Max(0, health.currentHealth - dmg);
            if (logDamage) Debug.Log($"[Player] daño {dmg} → hp={health.currentHealth}/{health.maxHealth}");
            currentHP = health.currentHealth; maxHP = health.maxHealth;
            RefreshHud();
            if (health.currentHealth <= 0) Die();
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;

        if (useLocalHP)
        {
            currentHP = Mathf.Min(maxHP, currentHP + amount);
            RefreshHud();
        }
        else if (health)
        {
            health.currentHealth = Mathf.Min(health.maxHealth, health.currentHealth + amount);
            currentHP = health.currentHealth; maxHP = health.maxHealth;
            RefreshHud();
        }
    }

    public void HealToFull()
    {
        if (useLocalHP)
        {
            currentHP = Mathf.Max(1, maxHP);
            RefreshHud();
        }
        else if (health)
        {
            health.currentHealth = Mathf.Max(1, health.maxHealth);
            currentHP = health.currentHealth; maxHP = health.maxHealth;
            RefreshHud();
        }
    }

    public void SetMaxHP(int newMax, bool healToFull = true)
    {
        newMax = Mathf.Max(1, newMax);

        if (useLocalHP)
        {
            maxHP = newMax;
            if (healToFull) currentHP = maxHP;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);
            RefreshHud();
        }
        else if (health)
        {
            health.maxHealth = newMax;
            if (healToFull) health.currentHealth = newMax;
            health.currentHealth = Mathf.Clamp(health.currentHealth, 0, health.maxHealth);
            currentHP = health.currentHealth; maxHP = health.maxHealth;
            RefreshHud();
        }
    }

   
    public void Die()
    {
        try { EventBus.RaisePlayerDied(); } catch { }
        try { GameFacade.I?.GameOver(); } catch { }
        gameObject.SetActive(false);
    }

    private void RefreshHud()
    {
        try { EventBus.RaisePlayerHPChanged(CurrentHP, MaxHP); } catch { }
    }
}
