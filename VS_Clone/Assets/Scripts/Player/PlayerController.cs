using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 input;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input.Normalize();
    }

    private void FixedUpdate()
    {
        
        float moveBonus = GameSession.Instance?.Stats.Get("MoveSpeed") ?? 0f;
        float finalSpeed = Mathf.Max(0f, moveSpeed + moveBonus);

        rb.velocity = input * finalSpeed;
    }
}
