using UnityEngine;

[DisallowMultipleComponent]
public class PlayerWeaponHolder : MonoBehaviour
{
    [Header("Equip Options")]

    public IWeapon Current { get; private set; }

    public void Equip(IWeapon newWeapon)
    {
        if (newWeapon == null) return;
        // 기존 이벤트 언바인딩 필요 시 여기서 처리
        Current = newWeapon;
        // UI/사운드 연동은 외부에서 Current 이벤트 구독
    }
}
