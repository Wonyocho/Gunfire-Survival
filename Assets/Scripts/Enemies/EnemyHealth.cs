using UnityEngine;

[DisallowMultipleComponent]
public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHP = 10f;
    private float hp;

    // 점수 계산/통계를 위해 외부에서 읽기만 가능하도록 노출
    public float MaxHP => maxHP;
    public float CurrentHP => hp;

    // 사망 이벤트: 컨트롤러/서비스가 구독하여 점수/킬 수를 갱신
    public event System.Action<EnemyHealth> Died;

    void Awake() => hp = maxHP;

    public void TakeDamage(float dmg)
    {
        hp -= dmg;
        if (hp <= 0f) Die();
    }

    void Die()
    {
        // 적 사망 알림
        Died?.Invoke(this);
        // 적 사망 시 그냥 파괴 (스포너의 OnDestroyNotify가 alive-- 처리함)
        Destroy(gameObject);
    }
}
