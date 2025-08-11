using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable, IHealthSource
{
    [SerializeField] private float maxHP = 100f;
    private float hp;

    public float HP => hp;
    public float MaxHP => maxHP;

    // IHealthSource 구현
    public float CurrentHP => hp;
    public float MaxHPForUI => maxHP; // 별칭(인터페이스와 충돌 방지용 주석)
    public event System.Action<float, float> HealthChanged;

    void Awake()
    {
        hp = maxHP;
        // 초기 상태 통지
        HealthChanged?.Invoke(hp, maxHP);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryApplyDamageFrom(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryApplyDamageFrom(other);
    }

    void TryApplyDamageFrom(Component colliderOrTrigger)
    {
        if (colliderOrTrigger == null) return;
        var src = colliderOrTrigger.GetComponentInParent<IDamageSource>();
        if (src != null)
        {
            TakeDamage(src.Damage);
            return;
        }
    }

    public void TakeDamage(float dmg)
    {
        if (dmg <= 0) return;
        hp -= dmg;
        if (hp <= 0)
        {
            hp = 0;
            HealthChanged?.Invoke(hp, maxHP);
            Die();
            return;
        }
        Debug.Log($"Player took {dmg} damage, HP is now {hp}");
        HealthChanged?.Invoke(hp, maxHP);
    }

    void Die()
    {
        Debug.Log("Player has died!");
        var survivalTimer = FindFirstObjectByType<SurvivalTimer>();
        if (survivalTimer != null)
        {
            survivalTimer.StopTimer();
        }
        Time.timeScale = 0f;
        gameObject.SetActive(false);
    }
}
