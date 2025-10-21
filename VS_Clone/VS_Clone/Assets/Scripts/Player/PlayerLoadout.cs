using UnityEngine;

public class PlayerLoadout : MonoBehaviour
{
    [SerializeField] private WeaponFactory weaponFactory;
    [SerializeField] private WeaponConfig startingWeapon; 

    private void Start()
    {
        if (!weaponFactory) { Debug.LogError("[PlayerLoadout] Falta WeaponFactory", this); return; }
        if (!startingWeapon) { Debug.LogError("[PlayerLoadout] Falta startingWeapon", this); return; }

        weaponFactory.CreateOn(transform, startingWeapon);
    }
}
