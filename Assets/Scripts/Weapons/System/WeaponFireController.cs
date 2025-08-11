using UnityEngine;

[DisallowMultipleComponent]
public class WeaponFireController : MonoBehaviour, IFireEventSource
{
    // 외부 VFX/사운드가 구독할 발사 이벤트
    public event System.Action<Vector2, Vector2, IWeapon> OnFired;

    [Header("Weapon")]
    [SerializeField] StartWeaponType slot1StartWeapon = StartWeaponType.R700; // 슬롯1 시작 무기
    [SerializeField] StartWeaponType slot2StartWeapon = StartWeaponType.M1911; // 슬롯2 시작 무기

    [Header("Refs")]
    [SerializeField] Transform muzzle;        // 비우면 transform 사용
    [SerializeField] LayerMask hitMask;       // Enemy 레이어 권장

    // 시스템 구성요소
    IFireInput input;
    IAimProvider aimProvider;
    WeaponSlots slots;
    FireService fireService;

    void Awake()
    {
        if (!muzzle) muzzle = transform;

        // 시작 무기 장착 (슬롯1/슬롯2)
        var weaponSlot1 = WeaponFactory.Create(slot1StartWeapon);
        var weaponSlot2 = WeaponFactory.Create(slot2StartWeapon);
        slots = new WeaponSlots(weaponSlot1, weaponSlot2);

        // 입력/조준 공급자 구성 (필요 시 다른 구현으로 교체 가능)
        input = new MouseKeyboardFireInput();
        aimProvider = new MouseAimProvider(muzzle, Camera.main, transform);

        // 발사 서비스 구성 및 이벤트 포워딩
        fireService = new FireService(input, aimProvider, slots, hitMask);
        fireService.OnFired += (origin, dir, weapon) => OnFired?.Invoke(origin, dir, weapon);
    }

    void Update()
    {
        // 무기 상태 업데이트(둘 다 갱신하여 재장전/쿨타임 유지)
        slots?.Update(Time.deltaTime);

        // 발사/슬롯 전환/리로드 등 입력 처리 + 발사 수행
        fireService?.Tick();
    }
}
