using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHP = 100f;
    private float hp;

    public float HP => hp;
    public float MaxHP => maxHP;

    void Awake()
    {
        hp = maxHP;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Enemy 레이어인지 확인 (레이어 설정이 되어 있다면)
        bool isEnemyLayer = (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"));
        Debug.Log($"[PlayerCollision] Player '{name}' collided with '{collision.collider.name}' (Layer: {LayerMask.LayerToName(collision.gameObject.layer)})");
        
        // EnemyController 또는 EnemyMovement 컴포넌트 찾기 (부모 포함)
        var enemy = collision.gameObject.GetComponentInParent<EnemyController>();
        var enemyMovement = collision.gameObject.GetComponentInParent<EnemyMovement>();
        
        if (enemy != null)
        {
            Debug.Log($"[PlayerDamage] Player hit by enemy '{enemy.name}' for {enemy.Damage} damage");
            TakeDamage(enemy.Damage);
        }
        else if (enemyMovement != null)
        {
            Debug.Log($"[PlayerDamage] Player hit by enemy movement '{enemyMovement.name}' for {enemyMovement.DamageOnCollision} damage");
            TakeDamage(enemyMovement.DamageOnCollision);
        }
        else if (isEnemyLayer || collision.gameObject.name.Contains("Enemy"))
        {
            Debug.LogWarning($"[PlayerCollision] Enemy object '{collision.gameObject.name}' has no damage component - taking default 10 damage");
            TakeDamage(10f); // 기본 데미지
        }
        else
        {
            Debug.Log($"[PlayerCollision] Non-enemy collision with '{collision.gameObject.name}'");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Enemy 레이어인지 확인
        bool isEnemyLayer = (other.gameObject.layer == LayerMask.NameToLayer("Enemy"));
        Debug.Log($"[PlayerTrigger] Player '{name}' trigger-entered '{other.name}' (Layer: {LayerMask.LayerToName(other.gameObject.layer)})");
        
        // EnemyController 또는 EnemyMovement 컴포넌트 찾기 (부모 포함)
        var enemy = other.gameObject.GetComponentInParent<EnemyController>();
        var enemyMovement = other.gameObject.GetComponentInParent<EnemyMovement>();
        
        if (enemy != null)
        {
            Debug.Log($"[PlayerDamage] Player trigger-hit by enemy '{enemy.name}' for {enemy.Damage} damage");
            TakeDamage(enemy.Damage);
        }
        else if (enemyMovement != null)
        {
            Debug.Log($"[PlayerDamage] Player trigger-hit by enemy movement '{enemyMovement.name}' for {enemyMovement.DamageOnCollision} damage");
            TakeDamage(enemyMovement.DamageOnCollision);
        }
        else if (isEnemyLayer || other.gameObject.name.Contains("Enemy"))
        {
            Debug.LogWarning($"[PlayerTrigger] Enemy object '{other.gameObject.name}' has no damage component - taking default 10 damage");
            TakeDamage(10f); // 기본 데미지
        }
        else
        {
            Debug.Log($"[PlayerTrigger] Non-enemy trigger with '{other.gameObject.name}'");
        }
    }

    public void TakeDamage(float dmg)
    {
        if (dmg <= 0) return;
        hp -= dmg;
        if (hp <= 0)
        {
            hp = 0;
            Die();
        }
        Debug.Log($"Player took {dmg} damage, HP is now {hp}");
    }

    void Die()
    {
        Debug.Log("Player has died!");
        
        // 생존 타이머 정지
        var survivalTimer = FindFirstObjectByType<SurvivalTimer>();
        if (survivalTimer != null)
        {
            survivalTimer.StopTimer();
        }
        
        // TODO: 게임 오버 처리 로직 추가
        // 일단은 게임 오브젝트를 비활성화합니다.
        Time.timeScale = 0f; // 시간을 멈춰서 게임을 정지시킵니다.
        gameObject.SetActive(false);
    }
}
