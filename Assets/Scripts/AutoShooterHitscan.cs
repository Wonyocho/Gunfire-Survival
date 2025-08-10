using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class AutoShooterHitscan : MonoBehaviour
{
    // 무기 시스템(IWeapon) 기반으로 리팩터링
    [Header("Weapon")]
    [SerializeField] bool startWithM1911 = true; // 기본 장착 무기
    [SerializeField, Tooltip("타겟 탐색 범위")] float targetRange = 35f;
    private IWeapon weapon; // 런타임 인스턴스(M1911 등)

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

        // 기본 무기 장착
        if (startWithM1911)
        {
            weapon = new M1911();
        }
    }

    void Update()
    {
        // 무기 업데이트(쿨다운/리로드 등)
        weapon?.Update(Time.deltaTime);

        var target = FindClosestEnemyInRange(targetRange);
        if (!target || weapon == null) return;

        Vector2 origin = muzzle ? (Vector2)muzzle.position : (Vector2)transform.position;
        Vector2 toTarget = (Vector2)target.transform.position - origin;
        float dist = toTarget.magnitude;
        Vector2 dir = (dist > 0.0001f) ? (toTarget / dist) : Vector2.right;

        // 발사 시도(IWeapon이 발사간격/탄약/리로드 관리)
        bool fired = weapon.CanFire && weapon.TryFire(origin, dir, hitMask);
        if (!fired) return;

        // ---- VFX: 라인 길이 계산 (무기 데미지 기반) ----
        float byDamage = lineBaseLength + weapon.Damage * lineLengthPerDamage;
        if (minVisualLength > 0f) byDamage = Mathf.Max(byDamage, minVisualLength);
        float visualMax = byDamage;

        if (clampToHitPoint)
        {
            // 시각용으로만 레이캐스트(피해는 이미 IWeapon 내부에서 처리됨)
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

    EnemyHealth FindClosestEnemyInRange(float range)
    {
        EnemyHealth closest = null;
        float bestSqr = range * range;
        Vector2 origin = muzzle ? (Vector2)muzzle.position : (Vector2)transform.position;

        foreach (var e in EnemyRegistry.All)
        {
            if (!e || !e.isActiveAndEnabled) continue;
            float sqr = ((Vector2)e.transform.position - origin).sqrMagnitude;
            if (sqr <= bestSqr)
            {
                bestSqr = sqr;
                closest = e;
            }
        }
        return closest;
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
