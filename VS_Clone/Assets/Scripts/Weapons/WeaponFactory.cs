using UnityEngine;

public class WeaponFactory : MonoBehaviour
{
    [SerializeField] private Pool projectilePool;
    [SerializeField] private GameObject weaponPrefab;

    public Weapon CreateOn(Transform owner, WeaponConfig config)
    {
        if (owner == null || config == null)
        {
            Debug.LogError("[WeaponFactory] Owner o Config nulos.");
            return null;
        }
        if (projectilePool == null)
        {
            Debug.LogError("[WeaponFactory] Falta asignar projectilePool en el Inspector.");
            return null;
        }

        GameObject go;
        if (weaponPrefab != null)
        {
            go = Instantiate(weaponPrefab, owner);
        }
        else
        {
            go = new GameObject($"Weapon_{config.id}");
            go.transform.SetParent(owner, worldPositionStays: false);
        }

        var weapon = go.GetComponent<Weapon>();
        if (weapon == null) weapon = go.AddComponent<Weapon>();

        weapon.SetOwner(owner);
        weapon.SetConfig(config);
        weapon.SetProjectilePool(projectilePool);

        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;

        return weapon;
    }
}