using UnityEngine;


public class AddWeaponUpgradeConfig : UpgradeConfig
{
    
    public WeaponConfig weaponConfig;

    public override void Apply(GameSession session, int newLevel)
    {
        if (weaponConfig == null)
        {
            Debug.LogWarning("[AddWeaponUpgrade] weaponConfig nulo");
            return;
        }

        var playerGo = GameObject.FindWithTag("Player");
        if (!playerGo) { Debug.LogError("[AddWeaponUpgrade] No se encontró Player"); return; }

        var factory = Object.FindObjectOfType<WeaponFactory>();
        if (!factory) { Debug.LogError("[AddWeaponUpgrade] No se encontró WeaponFactory"); return; }

        if (newLevel == 1)
        {
           
            factory.CreateOn(playerGo.transform, weaponConfig);
            Debug.Log($"[AddWeaponUpgrade] Desbloqueada arma {weaponConfig.name} (Lv 1/{maxLevel})");
        }
        else
        {
           
            session.Stats.Add($"weapon_{weaponConfig.id}_damage", weaponConfig.damage * 0.1f);
            Debug.Log($"[AddWeaponUpgrade] Mejora de {weaponConfig.name} (Lv {newLevel}/{maxLevel})");
        }
    }
}


