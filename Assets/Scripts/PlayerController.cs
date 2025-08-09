using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerHealth))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;

    private Rigidbody2D rb;
    private Vector2 input;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Rigidbody 설정 보정
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // 신/구 Input System 상관없이 동작
        float x = (Input.GetKey(KeyCode.D) ? 1 : 0) + (Input.GetKey(KeyCode.A) ? -1 : 0);
        float y = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0);

        input = new Vector2(x, y).normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = input * moveSpeed;
    }
}
