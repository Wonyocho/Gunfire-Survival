using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyMovement : MonoBehaviour, IDamageSource
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float damageOnCollision = 10f;

    public float DamageOnCollision => damageOnCollision;
    public float Damage => damageOnCollision; // IDamageSource 구현

    private Rigidbody2D rb;
    private Transform playerTarget;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTarget = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player GameObject has the 'Player' tag.");
        }
    }

    void FixedUpdate()
    {
        if (playerTarget != null)
        {
            Vector2 direction = (playerTarget.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어와의 충돌만 로깅(실제 데미지는 PlayerHealth에서 일원화 처리)
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"[EnemyCollision] Enemy '{name}' collided with Player '{collision.gameObject.name}'");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"[EnemyTrigger] Enemy '{name}' trigger-entered Player '{other.gameObject.name}'");
        }
    }
}
