using TMPro;
using UnityEngine;

/// <summary>
/// 플레이어 체력을 TextMeshProUGUI에 바인딩하는 프레젠터.
/// SRP: 표시만 담당. DIP: IHealthSource에만 의존.
/// </summary>
[DisallowMultipleComponent]
public class PlayerHealthHudPresenter : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Source (assign PlayerHealth or any IHealthSource implementor)")]
    [Tooltip("IHealthSource를 구현한 컴포넌트(예: PlayerHealth)를 드래그하세요. 비워두면 씬에서 자동 탐색합니다.")]
    [SerializeField] private MonoBehaviour healthSourceBehaviour;

    private IHealthSource _source;

    void Awake()
    {
        if (!healthText)
        {
            // 동일 오브젝트에 붙은 TMP를 자동 연결 시도
            healthText = GetComponent<TextMeshProUGUI>();
            if (!healthText)
                Debug.LogWarning("PlayerHealthHudPresenter: healthText is not assigned.");
        }
    }

    void OnEnable()
    {
        ResolveSource();
        if (_source == null)
        {
            Debug.LogError("PlayerHealthHudPresenter: IHealthSource not found. Assign healthSourceBehaviour or ensure PlayerHealth exists in scene.");
            return;
        }

        _source.HealthChanged += OnHealthChanged;
        // 초기 표시
        OnHealthChanged(_source.CurrentHP, _source.MaxHP);
    }

    void OnDisable()
    {
        if (_source != null)
        {
            _source.HealthChanged -= OnHealthChanged;
        }
    }

    void ResolveSource()
    {
        if (healthSourceBehaviour != null && healthSourceBehaviour is IHealthSource s1)
        {
            _source = s1;
            return;
        }

        // 미할당 시 씬에서 자동 탐색 (최초 1개 가정)
        var found = FindAnyObjectByType<PlayerHealth>();
        _source = found as IHealthSource;
    }

    void OnHealthChanged(float current, float max)
    {
        if (!healthText) return;
        if (max <= 0f)
        {
            healthText.text = "HP: -";
            return;
        }
        // 소수점 없는 정수 표기. 필요하면 포맷 변경 가능
        healthText.text = $"{current:0}/{max:0}";
    }
}
