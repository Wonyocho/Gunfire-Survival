using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerHealth))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    
    [Header("World Bounds")]
    [SerializeField] private BoxCollider2D worldBounds; // EnemySpawner와 같은 WorldBounds 할당

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
        Vector2 newVelocity = input * moveSpeed;
        rb.linearVelocity = newVelocity;
        
        // 경계 제한 적용
        ClampPlayerToWorldBounds();
    }
    
    void ClampPlayerToWorldBounds()
    {
        if (!worldBounds) return;
        
        Vector2 currentPos = transform.position;
        var bounds = worldBounds.bounds;
        
        // 경계 내부로 제한 (약간의 여유 공간 0.5f)
        float clampedX = Mathf.Clamp(currentPos.x, bounds.min.x + 0.5f, bounds.max.x - 0.5f);
        float clampedY = Mathf.Clamp(currentPos.y, bounds.min.y + 0.5f, bounds.max.y - 0.5f);
        
        Vector2 clampedPos = new Vector2(clampedX, clampedY);
        
        // 위치가 변경되었다면 적용
        if (Vector2.Distance(currentPos, clampedPos) > 0.01f)
        {
            transform.position = clampedPos;
            // 벽에 닿으면 속도를 0으로 (미끄러짐 방지)
            rb.linearVelocity = Vector2.zero;
        }
    }
}
