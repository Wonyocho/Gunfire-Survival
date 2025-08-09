using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float damageOnCollision = 10f;

    public float DamageOnCollision => damageOnCollision;

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
        // "Player" 태그를 가진 오브젝트를 찾습니다.
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
            rb.linearVelocity = Vector2.zero; // 플레이어가 없으면 정지
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Enemy끼리 충돌하는 경우는 무시
        if (collision.gameObject.GetComponentInParent<EnemyMovement>() != null || 
            collision.gameObject.GetComponentInParent<EnemyController>() != null ||
            collision.gameObject.GetComponentInParent<EnemyHealth>() != null)
        {
            Debug.Log($"[EnemyCollision] Enemy '{name}' collided with another enemy '{collision.gameObject.name}' - ignoring");
            return;
        }

        // 플레이어와의 충돌만 처리
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"[EnemyCollision] Enemy '{name}' collided with Player '{collision.gameObject.name}'");
            // 충돌 로그만 남기고, 실제 데미지는 PlayerHealth에서 처리하도록 위임
        }
        else
        {
            Debug.Log($"[EnemyCollision] Enemy '{name}' collided with non-player/non-enemy '{collision.gameObject.name}'");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Enemy끼리 트리거는 무시
        if (other.gameObject.GetComponentInParent<EnemyMovement>() != null || 
            other.gameObject.GetComponentInParent<EnemyController>() != null ||
            other.gameObject.GetComponentInParent<EnemyHealth>() != null)
        {
            return;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"[EnemyTrigger] Enemy '{name}' trigger-entered Player '{other.gameObject.name}'");
            // 트리거 충돌도 PlayerHealth에서 처리하도록 위임
        }
    }
}
