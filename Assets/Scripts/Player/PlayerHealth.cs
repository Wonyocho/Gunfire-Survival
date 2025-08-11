using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
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
            Die();
        }
        Debug.Log($"Player took {dmg} damage, HP is now {hp}");
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
