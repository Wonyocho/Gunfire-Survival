using UnityEngine;

/// <summary>
/// 데미지를 제공하는 공격 주체. 충돌/공격 시 피해량 질의를 표준화합니다.
/// </summary>
public interface IDamageSource
{
    float Damage { get; }
}
