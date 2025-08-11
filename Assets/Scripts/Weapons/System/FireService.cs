using UnityEngine;

/// <summary>
/// 발사 조건을 검사하고 실제 무기 발사를 수행. 발사 이벤트를 외부로 전달.
/// </summary>
public class FireService
{
    public System.Action<Vector2, Vector2, IWeapon> OnFired;

    readonly IFireInput input;
    readonly IAimProvider aim;
    readonly WeaponSlots slots;
    readonly LayerMask hitMask;

    public FireService(IFireInput input, IAimProvider aim, WeaponSlots slots, LayerMask hitMask)
    {
        this.input = input;
        this.aim = aim;
        this.slots = slots;
        this.hitMask = hitMask;
    }

    public void Tick()
    {
        // 슬롯 관리
        if (input.SwitchSlotPressed) slots.Switch();
        if (input.ReloadPressed) slots.StartReload();

        // 조준 계산
        aim.GetAim(out var origin, out var dir);
        if (dir == Vector2.zero) return;

        var weapon = slots.Current;
        if (weapon == null) return;

        // 발사 입력/조건 검사
        if (!input.IsFiring) return;
        if (!weapon.CanFire) return;

        weapon.TryFire(origin, dir, hitMask);
        OnFired?.Invoke(origin, dir, weapon);
    }
}
