using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public enum WeaponType { Sword, Axe, MagicWand } // Tipos de armas
    public WeaponType weaponType;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Si el jugador toca el arma
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                // Cambia la estrategia de ataque según el tipo de arma
                switch (weaponType)
                {
                    case WeaponType.Sword:
                        player.SetAttackStrategy(new SwordAttack());
                        break;
                    case WeaponType.Axe:
                        player.SetAttackStrategy(new AxeAttack());
                        break;
                    case WeaponType.MagicWand:
                        player.SetAttackStrategy(new MagicWandAttack());
                        break;
                }

                // ?? Ya NO se destruye el arma: permanece en escena
                Debug.Log("Arma activada, pero permanece en escena.");
            }
        }
    }
}