using UnityEngine;

public class AxeAttack : IAttackStrategy
{
    public void Attack()
    {
        Debug.Log("¡Atacas con un hacha! Golpe pesado y lento."); // Mensaje específico de hacha
    }
}
