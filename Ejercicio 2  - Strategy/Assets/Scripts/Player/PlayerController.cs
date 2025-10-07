using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IAttackStrategy attackStrategy; // Estrategia actual del jugador

    public float speed = 5f; // Velocidad de movimiento

    void Start()
    {
        // Se puede asignar una estrategia por defecto
        attackStrategy = new SwordAttack();
    }

    void Update()
    {
        // Movimiento básico
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveX, 0f, moveZ);
        transform.Translate(movement * speed * Time.deltaTime);

        // Ataque al presionar espacio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (attackStrategy != null)
            {
                attackStrategy.Attack(); // Ejecuta el método de ataque actual
            }
        }
    }

    public void SetAttackStrategy(IAttackStrategy newStrategy)
    {
        attackStrategy = newStrategy; // Cambia la estrategia actual
        Debug.Log("Nueva arma equipada: " + newStrategy.GetType().Name);
    }
}
