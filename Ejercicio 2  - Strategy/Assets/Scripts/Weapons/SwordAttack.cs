using UnityEngine;

public class SwordAttack : IAttackStrategy
{
    public void Attack()
    {
        Debug.Log("�Atacas con una espada! Corte r�pido y preciso."); // Mensaje espec�fico de espada
    }
}