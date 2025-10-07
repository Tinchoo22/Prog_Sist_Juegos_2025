using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public string enemyName; // Nombre del enemigo
    public float speed;      // Velocidad de movimiento

    public abstract void Attack(); // M�todo abstracto que cada enemigo implementar�
}