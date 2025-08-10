using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class AutoShooterHitscan : MonoBehaviour
{
    // 무기 시스템(IWeapon) 기반으로 리팩터링
    public enum StartWeaponType { M1911, UZI, M4A1, R700, M249 }

    [Header("Weapon")]
    [SerializeField] StartWeaponType slot1StartWeapon = StartWeaponType.R700; // 슬롯1 시작 무기
    [SerializeField] StartWeaponType slot2StartWeapon = StartWeaponType.M1911; // 슬롯2 시작 무기
    private IWeapon weaponSlot1; // 슬롯1 인스턴스
    private IWeapon weaponSlot2; // 슬롯2 인스턴스
    private int currentSlot = 1; // 1 또는 2
    private IWeapon Weapon => currentSlot == 1 ? weaponSlot1 : weaponSlot2;

    [Header("Refs")]
    [SerializeField] Transform muzzle;        // 비우면 transform 사용
    [SerializeField] LayerMask hitMask;       // Enemy 레이어 권장
    [SerializeField] LineRenderer shotLine;   // 하얀 선

    [Header("VFX")]
    [SerializeField] float lineDuration = 0.05f;

    [Header("Line length by damage")]
    [Tooltip("라인 기본 길이(월드 단위)")]
    [SerializeField] float lineBaseLength = 3f;
    [Tooltip("데미지 1당 추가 길이(월드 단위)")]
    [SerializeField] float lineLengthPerDamage = 0.25f;
    [Tooltip("최소 라인 길이 보장(0이면 사용 안 함)")]
    [SerializeField] float minVisualLength = 0f;
    [Tooltip("실제 명중 지점보다 길게 그리지 않음 (시각용 보조 레이캐스트)")]
    [SerializeField] bool clampToHitPoint = true;

    Coroutine shotRoutine;

    void Awake()
    {
        if (!muzzle) muzzle = transform;
        if (!shotLine) shotLine = GetComponent<LineRenderer>();
        if (shotLine)
        {
            shotLine.positionCount = 2;
            shotLine.enabled = false;
        }

        // 시작 무기 장착 (슬롯1/슬롯2)
        weaponSlot1 = CreateWeapon(slot1StartWeapon);
        weaponSlot2 = CreateWeapon(slot2StartWeapon);
        currentSlot = 1;
    }

    void Update()
    {
        // 무기 상태 업데이트(둘 다 갱신하여 재장전/쿨타임 유지)
        weaponSlot1?.Update(Time.deltaTime);
        weaponSlot2?.Update(Time.deltaTime);

        // 슬롯 전환(Q)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleSlot();
        }

        // 재장전(R)
        if (Input.GetKeyDown(KeyCode.R))
        {
            Weapon?.StartReload();
        }

        var weapon = Weapon;
        if (weapon == null) return;

        // 마우스 좌클릭 유지 시 발사
        if (!Input.GetMouseButton(0)) return;

        Vector2 origin = muzzle ? (Vector2)muzzle.position : (Vector2)transform.position;
        var cam = Camera.main;
        if (!cam) return;

        Vector3 mouse = Input.mousePosition;
        // 플레이어와 동일 평면(z)에서의 월드 좌표로 변환
        float planeZ = transform.position.z;
        mouse.z = planeZ - cam.transform.position.z;
        Vector3 mouseWorld = cam.ScreenToWorldPoint(mouse);

        Vector2 toMouse = (Vector2)mouseWorld - origin;
        float dist = toMouse.magnitude;
        if (dist <= 0.0001f) return;
        Vector2 dir = toMouse / Mathf.Max(0.0001f, dist);

        // 발사 시도(IWeapon이 발사간격/탄약/리로드 관리)
        // VFX는 적 적중 여부와 무관하게 항상 표시되도록 TryFire 결과와 상관없이 진행
        if (!weapon.CanFire) return;
        weapon.TryFire(origin, dir, hitMask);

        // ---- VFX: 라인 길이 계산 (무기 데미지 기반) ----
        float byDamage = lineBaseLength + weapon.Damage * lineLengthPerDamage;
        if (minVisualLength > 0f) byDamage = Mathf.Max(byDamage, minVisualLength);
        float visualMax = byDamage;

        if (clampToHitPoint)
        {
            // 시각용 레이캐스트로 명중 지점까지 캡(실제 피해는 무기에서 처리됨)
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, 100f, hitMask);
            if (hit.collider)
            {
                visualMax = Mathf.Min(visualMax, hit.distance);
            }
        }

        Vector2 visualEnd = origin + dir * visualMax;

        // ---- 라인 번쩍 ----
        if (shotLine)
        {
            if (shotRoutine != null) StopCoroutine(shotRoutine);
            shotRoutine = StartCoroutine(ShowShotLine(origin, visualEnd));
        }
    }

    private void ToggleSlot()
    {
        currentSlot = currentSlot == 1 ? 2 : 1;
        // 필요 시 슬롯 전환 사운드/이벤트 트리거 가능
        // Debug.Log($"Switched to Slot {currentSlot}");
    }

    private IWeapon CreateWeapon(StartWeaponType type)
    {
        switch (type)
        {
            case StartWeaponType.M1911: return new M1911();
            case StartWeaponType.UZI:   return new UZI();
            case StartWeaponType.M4A1:  return new M4A1();
            case StartWeaponType.M249:  return new M249();
            case StartWeaponType.R700:
            default:                    return new R700();
        }
    }

    IEnumerator ShowShotLine(Vector2 start, Vector2 end)
    {
        shotLine.enabled = true;
        shotLine.SetPosition(0, start);
        shotLine.SetPosition(1, end);
        yield return new WaitForSeconds(lineDuration);
        shotLine.enabled = false;
    }
}
