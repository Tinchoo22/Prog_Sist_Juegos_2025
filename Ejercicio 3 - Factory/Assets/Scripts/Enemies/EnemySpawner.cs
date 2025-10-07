using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Presionar E genera un enemigo
        {
            Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f)); // Posición aleatoria
            Enemy nuevoEnemigo = EnemyFactory.CreateRandomEnemy(randomPos); // Usa la fábrica

            if (nuevoEnemigo != null)
            {
                Debug.Log("Se generó un " + nuevoEnemigo.enemyName);
                nuevoEnemigo.Attack(); // Ejecuta su comportamiento propio
            }
        }
    }
}
