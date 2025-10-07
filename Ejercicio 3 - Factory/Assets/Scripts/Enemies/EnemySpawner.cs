using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Presionar E genera un enemigo
        {
            Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f)); // Posici�n aleatoria
            Enemy nuevoEnemigo = EnemyFactory.CreateRandomEnemy(randomPos); // Usa la f�brica

            if (nuevoEnemigo != null)
            {
                Debug.Log("Se gener� un " + nuevoEnemigo.enemyName);
                nuevoEnemigo.Attack(); // Ejecuta su comportamiento propio
            }
        }
    }
}
