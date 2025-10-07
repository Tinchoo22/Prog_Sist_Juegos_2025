using UnityEngine;

public class Vampire : Enemy
{
    void Start()
    {
        enemyName = "Vampire";
        speed = 3f;
    }

    public override void Attack()
    {
        Debug.Log("El Vampiro te chupa la sangre rápidamente...");
    }
}