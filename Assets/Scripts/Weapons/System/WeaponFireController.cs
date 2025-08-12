using UnityEngine;

[DisallowMultipleComponent]
public class WeaponFireController : MonoBehaviour, IFireEventSource, IWeaponProvider
{
    // 외부 VFX/사운드가 구독할 발사 이벤트 (조준 방향)
    public event System.Action<Vector2, Vector2, IWeapon> OnFired;
    
    // 실제 발사 방향 이벤트 (Spread 적용됨)
    public event System.Action<Vector2, Vector2, IWeapon> OnActualShot;

    // UI가 구독할 무기 전환 이벤트
    public event System.Action<IWeapon> OnWeaponSwitched;

    [Header("Weapon")]
    [SerializeField] StartWeaponType slot1StartWeapon = StartWeaponType.Mzi; // 슬롯1 시작 무기
    [SerializeField] StartWeaponType slot2StartWeapon = StartWeaponType.Mzi_Pro; // 슬롯2 시작 무기 (HG 제거)

    [Header("Refs")]
    [Tooltip("발사 원점. 비우면 Owner를 사용합니다.")]
    [SerializeField] Transform muzzle;        // 비우면 owner 사용
    [Tooltip("조준 기준이 되는 소유자(보통 Player). 비우면 자동 탐색(PlayerController 또는 태그 Player)")]
    [SerializeField] Transform owner;         // Player Transform
    [SerializeField] LayerMask hitMask;       // Enemy 레이어 권장

    // 시스템 구성요소
    IFireInput input;
    IAimProvider aimProvider;
    WeaponSlots slots;
    FireService fireService;

    public IWeapon CurrentWeapon => slots?.Current;

    void Awake()
    {
        // Owner 자동 할당 시도
        if (!owner)
        {
            var pc = FindAnyObjectByType<PlayerController>();
            if (pc) owner = pc.transform;
            else {
                var playerGO = GameObject.FindGameObjectWithTag("Player");
                if (playerGO) owner = playerGO.transform;
            }
        }

        // Muzzle 비어있으면 owner 사용(최소한 플레이어 위치에서 발사)
        if (!muzzle) muzzle = owner ? owner : transform;

        // 시작 무기 장착 (슬롯1/슬롯2)
        var weaponSlot1 = WeaponFactory.Create(slot1StartWeapon);
        var weaponSlot2 = WeaponFactory.Create(slot2StartWeapon);
        slots = new WeaponSlots(weaponSlot1, weaponSlot2);
        
        // 무기의 실제 발사 이벤트 구독
        BindWeaponEvents(weaponSlot1);
        BindWeaponEvents(weaponSlot2);

        // 입력/조준 공급자 구성 (필요 시 다른 구현으로 교체 가능)
        input = new MouseKeyboardFireInput();
        var cam = Camera.main;
        aimProvider = new MouseAimProvider(muzzle, cam, owner ? owner : transform);

        // 발사 서비스 구성 및 이벤트 포워딩
        fireService = new FireService(input, aimProvider, slots, hitMask);
        fireService.OnFired += (origin, dir, weapon) => OnFired?.Invoke(origin, dir, weapon);
    }
    
    void BindWeaponEvents(IWeapon weapon)
    {
        if (weapon is HitscanWeaponBase hitscanWeapon)
        {
            hitscanWeapon.OnActualShot += (origin, actualDir, w) => OnActualShot?.Invoke(origin, actualDir, w);
        }
    }

    void Update()
    {
        // 무기 상태 업데이트(둘 다 갱신하여 재장전/쿨타임 유지)
        slots?.Update(Time.deltaTime);

        int before = slots != null ? slots.CurrentIndex : 0;

        // 발사/슬롯 전환/리로드 등 입력 처리 + 발사 수행
        fireService?.Tick();

        // 슬롯 전환 탐지 후 이벤트 발송
        if (slots != null && slots.CurrentIndex != before)
        {
            OnWeaponSwitched?.Invoke(slots.Current);
        }
    }
}
