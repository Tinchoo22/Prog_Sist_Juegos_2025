using UnityEngine;

public class EnemyRegistry : MonoBehaviour
{
    public GameObject zombiePrefab;
    public GameObject vampirePrefab;
    public GameObject ghostPrefab;

    void Start()
    {
        EnemyFactory.RegisterEnemy("Zombie", zombiePrefab);
        EnemyFactory.RegisterEnemy("Vampire", vampirePrefab);
        EnemyFactory.RegisterEnemy("Ghost", ghostPrefab);
    }
}