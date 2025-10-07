using UnityEngine;

public class Zombie : Enemy
{
    void Start()
    {
        enemyName = "Zombie"; // Asigna el nombre
        speed = 1.5f;          // Velocidad propia
    }

    public override void Attack()
    {
        Debug.Log("El Zombie te muerde lentamente..."); // Mensaje único de ataque
    }
}
