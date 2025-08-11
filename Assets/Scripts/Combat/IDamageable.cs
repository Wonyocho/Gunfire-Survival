using UnityEngine;

/// <summary>
/// 데미지를 받을 수 있는 대상에 대한 계약. 무기/충돌 처리에서 타입 의존성을 제거합니다.
/// </summary>
public interface IDamageable
{
    void TakeDamage(float amount);
}
