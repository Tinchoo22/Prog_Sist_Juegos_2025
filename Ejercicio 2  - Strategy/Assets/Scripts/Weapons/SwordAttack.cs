using UnityEngine;

public class SwordAttack : IAttackStrategy
{
    public void Attack()
    {
        Debug.Log("¡Atacas con una espada! Corte rápido y preciso."); // Mensaje específico de espada
    }
}