using UnityEngine;

public class Collectible : MonoBehaviour
{
    void OnTriggerEnter(Collider other) // Se activa al entrar en contacto con otro collider
    {
        if (other.CompareTag("Player")) // Verifica si el objeto que lo toc� tiene la etiqueta "Player"
        {
            ScoreManager.Instance.AddPoint(); // Llama al m�todo para sumar punto
            Destroy(gameObject); // Destruye el objeto coleccionable
        }
    }
}