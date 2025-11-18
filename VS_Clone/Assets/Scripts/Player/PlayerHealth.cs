using UnityEngine;

[DisallowMultipleComponent]
public class PlayerHealth : MonoBehaviour
{
    
    public int maxHealth = 5;
    public int currentHealth;

    
    public float invulnTime = 0.5f;
    private float lastHitTime = -999f;

    PlayerStateMachine state;

    void Awake()
    {
        state = GetComponent<PlayerStateMachine>();
        currentHealth = Mathf.Max(1, maxHealth);
    }

    public void TakeDamage(int dmg)
    {
        if (Time.time - lastHitTime < invulnTime) return;
        lastHitTime = Time.time;

        dmg = Mathf.Max(0, dmg);
        currentHealth -= dmg;
        EventBus.RaisePlayerHPChanged(currentHealth, maxHealth);
        Debug.Log($"[PlayerHealth] Damage {dmg} → HP {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            EventBus.RaisePlayerHPChanged(currentHealth, maxHealth);
            if (state) state.Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + Mathf.Max(0, amount));
        EventBus.RaisePlayerHPChanged(currentHealth, maxHealth);
    }
}

