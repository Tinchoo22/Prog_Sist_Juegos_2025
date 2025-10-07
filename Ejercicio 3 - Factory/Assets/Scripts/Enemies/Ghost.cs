using UnityEngine;

public class Ghost : Enemy
{
    void Start()
    {
        enemyName = "Ghost";
        speed = 2.5f;
    }

    public override void Attack()
    {
        Debug.Log("El Fantasma atraviesa tu alma...");
    }
}