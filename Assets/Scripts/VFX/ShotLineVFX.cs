using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 발사 이벤트를 구독해 라인 렌더러로 총격 비주얼을 그립니다.
/// 실제 발사 방향(Spread 적용)을 표시합니다.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(LineRenderer))]
public class ShotLineVFX : MonoBehaviour
{
    [Header("Refs")]
    [Tooltip("발사 이벤트 소스: OnActualShot(Vector2, Vector2, IWeapon) 이벤트를 가진 임의의 컴포넌트")]
    [FormerlySerializedAs("shooter")]
    [SerializeField] MonoBehaviour fireEventSource; // 새 구조: 반사로 이벤트 구독, 레거시 필드 자동 마이그레이션

    [SerializeField] LayerMask clampMask;          // 시각용 히트 지점 캡(옵션)

    [Header("VFX Settings")]
    [SerializeField] float lineDuration = 0.05f;

    [Header("Line length by damage")]
    [SerializeField] float lineBaseLength = 3f;
    [SerializeField] float lineLengthPerDamage = 0.25f;
    [SerializeField] float minVisualLength = 0f;
    [Tooltip("실제 명중 지점까지로 시각 길이를 제한할지 여부")]
    [SerializeField] bool clampToHitPoint = true;

    LineRenderer line;
    Coroutine routine;

    // 반사용 구독 정보
    EventInfo onActualShotEventInfo;
    Delegate onActualShotHandler;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        if (line)
        {
            line.positionCount = 2;
            line.enabled = false;
        }
    }

    void OnEnable()
    {
        SetupSubscription();
    }

    void OnDisable()
    {
        TeardownSubscription();
    }

    // OnActualShot(Vector2 origin, Vector2 actualDir, IWeapon weapon) - Spread 적용된 실제 방향
    void HandleActualShot(Vector2 origin, Vector2 actualDir, IWeapon weapon)
    {
        if (!line || weapon == null) return;

        float visualMax = lineBaseLength + weapon.Damage * lineLengthPerDamage;
        if (minVisualLength > 0f) visualMax = Mathf.Max(visualMax, minVisualLength);

        if (clampToHitPoint)
        {
            var hit = Physics2D.Raycast(origin, actualDir, 100f, clampMask.value == 0 ? ~0 : clampMask);
            if (hit.collider)
                visualMax = Mathf.Min(visualMax, hit.distance);
        }

        Vector2 end = origin + actualDir * visualMax;
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(Show(origin, end));
    }

    IEnumerator Show(Vector2 start, Vector2 end)
    {
        line.enabled = true;
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        yield return new WaitForSeconds(lineDuration);
        line.enabled = false;
    }

    void SetupSubscription()
    {
        // 소스 미지정 시 부모에서 자동 탐색
        if (!fireEventSource)
            fireEventSource = FindFireEventSourceInParents();

        if (!fireEventSource)
            return;

        const string eventName = "OnActualShot";
        var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        onActualShotEventInfo = fireEventSource.GetType().GetEvent(eventName, flags);

        if (onActualShotEventInfo == null)
        {
            Debug.LogWarning($"ShotLineVFX: '{fireEventSource.GetType().Name}'에서 이벤트 {eventName}를 찾지 못했습니다.", this);
            return;
        }

        var expectedType = typeof(System.Action<Vector2, Vector2, IWeapon>);
        if (onActualShotEventInfo.EventHandlerType != expectedType)
        {
            Debug.LogWarning($"ShotLineVFX: 이벤트 시그니처가 다릅니다. 기대: {expectedType}", this);
            onActualShotEventInfo = null;
            return;
        }

        try
        {
            onActualShotHandler = Delegate.CreateDelegate(onActualShotEventInfo.EventHandlerType, this, nameof(HandleActualShot));
            onActualShotEventInfo.AddEventHandler(fireEventSource, onActualShotHandler);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"ShotLineVFX: 이벤트 구독 실패 - {e.Message}", this);
            onActualShotEventInfo = null;
            onActualShotHandler = null;
        }
    }

    void TeardownSubscription()
    {
        if (fireEventSource != null && onActualShotEventInfo != null && onActualShotHandler != null)
        {
            try { onActualShotEventInfo.RemoveEventHandler(fireEventSource, onActualShotHandler); }
            catch { /* ignore */ }
        }
        onActualShotEventInfo = null;
        onActualShotHandler = null;
    }

    MonoBehaviour FindFireEventSourceInParents()
    {
        var comps = GetComponentsInParent<MonoBehaviour>(true);
        var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        var expectedType = typeof(System.Action<Vector2, Vector2, IWeapon>);

        foreach (var c in comps)
        {
            if (c == this) continue;
            var ev = c.GetType().GetEvent("OnActualShot", flags);
            if (ev != null && ev.EventHandlerType == expectedType)
                return c;
        }
        return null;
    }
}
