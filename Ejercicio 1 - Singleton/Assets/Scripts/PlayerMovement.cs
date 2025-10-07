using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // Movimiento horizontal (A/D o flechas)
        float moveZ = Input.GetAxis("Vertical"); // Movimiento vertical (W/S o flechas)

        Vector3 movement = new Vector3(moveX, 0f, moveZ); // Crea un vector de movimiento en el plano XZ
        transform.Translate(movement * speed * Time.deltaTime); // Mueve al jugador
    }
}