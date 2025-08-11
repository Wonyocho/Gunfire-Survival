using UnityEngine;

/// <summary>
/// 현재 무기 상태를 외부에 제공하는 공급자 인터페이스.
/// DIP: UI는 이 인터페이스만 알면 됨.
/// </summary>
public interface IWeaponProvider
{
    IWeapon CurrentWeapon { get; }
    event System.Action<IWeapon> OnWeaponSwitched;
}
