using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class AutoShooterHitscan : MonoBehaviour
{
    [System.Serializable]
    public class WeaponStats
    {
        [Tooltip("한 발당 공격력")]       public float damage = 10f;
        [Tooltip("초당 발사 수")]        public float fireRate = 2f; // 0.5초/발
        [Tooltip("히트스캔 최대 사거리")] public float maxRange = 40f;
        [Tooltip("타겟 탐색 범위")]       public float targetRange = 35f;
    }

    [Header("Weapon")]
    public WeaponStats weapon = new WeaponStats();

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
    [Tooltip("실제 명중 지점보다 길게 그리지 않음 (권장)")]
    [SerializeField] bool clampToHitPoint = true;

    float nextFireTime;
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
    }

    void Update()
    {
        var target = FindClosestEnemyInRange(weapon.targetRange);
        if (!target) return;

        if (Time.time >= nextFireTime)
        {
            FireHitscan(target);
            nextFireTime = Time.time + 1f / Mathf.Max(0.0001f, weapon.fireRate);
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

    void FireHitscan(EnemyHealth target)
    {
        Vector2 origin = muzzle ? (Vector2)muzzle.position : (Vector2)transform.position;
        Vector2 toTarget = (Vector2)target.transform.position - origin;
        float dist = toTarget.magnitude;
        Vector2 dir = (dist > 0.0001f) ? (toTarget / dist) : Vector2.right;

        // 실제 레이 길이(사거리 캡)
        float rayDist = Mathf.Min(weapon.maxRange, dist + 0.1f);
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, rayDist, hitMask);

        // ---- 데미지 처리 ----
        if (hit.collider)
        {
            var eh = hit.collider.GetComponent<EnemyHealth>();
            if (eh) eh.TakeDamage(weapon.damage);
        }
        else if (dist <= weapon.maxRange)
        {
            // 레이가 못 잡아도 장애물 없다는 가정 하에서 보정(옵션)
            target.TakeDamage(weapon.damage);
        }

        // ---- 라인 길이 계산 (데미지 기반) ----
        float byDamage = lineBaseLength + weapon.damage * lineLengthPerDamage;
        if (minVisualLength > 0f) byDamage = Mathf.Max(byDamage, minVisualLength);

        // 시각적 최대길이: 기본적으로 사거리 내
        float visualMax = Mathf.Min(byDamage, weapon.maxRange);

        // 명중 지점보다 길게 그리고 싶지 않다면 히트 지점까지로 캡
        float hitDist = hit.collider ? hit.distance : rayDist;
        if (clampToHitPoint) visualMax = Mathf.Min(visualMax, hitDist);

        Vector2 visualEnd = origin + dir * visualMax;

        // ---- 라인 번쩍 ----
        if (shotLine)
        {
            if (shotRoutine != null) StopCoroutine(shotRoutine);
            shotRoutine = StartCoroutine(ShowShotLine(origin, visualEnd));
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
