using System;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyFactory
{
    // Diccionario que relaciona un nombre con un prefab de enemigo
    private static Dictionary<string, GameObject> enemyPrefabs = new Dictionary<string, GameObject>();

    // Método para registrar enemigos (se llama en Start de cada prefab loader)
    public static void RegisterEnemy(string enemyType, GameObject prefab)
    {
        if (!enemyPrefabs.ContainsKey(enemyType))
        {
            enemyPrefabs.Add(enemyType, prefab); // Guarda la referencia
        }
    }

    // Crea un enemigo aleatorio entre los registrados
    public static Enemy CreateRandomEnemy(Vector3 position)
    {
        if (enemyPrefabs.Count == 0)
        {
            Debug.LogError("No hay enemigos registrados en la fábrica.");
            return null;
        }

        List<string> keys = new List<string>(enemyPrefabs.Keys); // Obtiene los nombres registrados
        string randomKey = keys[UnityEngine.Random.Range(0, keys.Count)]; // Selecciona uno al azar
        GameObject prefab = enemyPrefabs[randomKey];

        GameObject instance = GameObject.Instantiate(prefab, position, Quaternion.identity); // Instancia el prefab
        return instance.GetComponent<Enemy>(); // Devuelve la clase Enemy asociada
    }
}