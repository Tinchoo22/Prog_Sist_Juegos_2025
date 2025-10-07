using UnityEngine;

public class MagicWandAttack : IAttackStrategy
{
    public void Attack()
    {
        Debug.Log("¡Lanzas un hechizo mágico desde la varita!"); // Mensaje específico de varita mágica
    }
}