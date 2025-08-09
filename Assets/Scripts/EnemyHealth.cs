using UnityEngine;

[DisallowMultipleComponent]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHP = 10f;
    private float hp;

    void Awake() => hp = maxHP;

    public void TakeDamage(float dmg)
    {
        hp -= dmg;
        if (hp <= 0f) Die();
    }

    void Die()
    {
        // 적 사망 시 그냥 파괴 (스포너의 OnDestroyNotify가 alive-- 처리함)
        Destroy(gameObject);
    }
}
