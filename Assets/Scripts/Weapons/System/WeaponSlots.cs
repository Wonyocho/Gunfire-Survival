using UnityEngine;

/// <summary>
/// 무기 슬롯(1/2) 상태를 관리하고, 업데이트/전환/리로드를 담당.
/// </summary>
public class WeaponSlots
{
    public IWeapon Slot1 { get; private set; }
    public IWeapon Slot2 { get; private set; }
    public int CurrentIndex { get; private set; } = 1; // 1 또는 2
    public IWeapon Current => CurrentIndex == 1 ? Slot1 : Slot2;

    public WeaponSlots(IWeapon slot1, IWeapon slot2)
    {
        Slot1 = slot1;
        Slot2 = slot2;
        CurrentIndex = 1;
    }

    public void Update(float delta)
    {
        Slot1?.Update(delta);
        Slot2?.Update(delta);
    }

    public void Switch()
    {
        CurrentIndex = CurrentIndex == 1 ? 2 : 1;
    }

    public void StartReload()
    {
        Current?.StartReload();
    }
}
