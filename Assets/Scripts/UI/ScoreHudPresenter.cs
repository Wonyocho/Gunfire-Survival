using TMPro;
using UnityEngine;

/// <summary>
/// 점수/킬 정보를 TextMeshProUGUI에 바인딩하는 프레젠터.
/// SRP: 표시만 담당. DIP: IScoreService에만 의존.
/// </summary>
[DisallowMultipleComponent]
public class ScoreHudPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private ScoreController scoreController; // 씬의 ScoreController 참조

    private IScoreService _service;

    void Awake()
    {
        if (!scoreText) Debug.LogWarning("ScoreHudPresenter: scoreText is not assigned");
        if (!killsText) Debug.LogWarning("ScoreHudPresenter: killsText is not assigned");
        if (!scoreController) scoreController = FindAnyObjectByType<ScoreController>();
    }

    void OnEnable()
    {
        if (!scoreController)
        {
            Debug.LogError("ScoreHudPresenter: ScoreController not found in scene.");
            return;
        }
        _service = scoreController.Service;
        if (_service == null)
        {
            Debug.LogError("ScoreHudPresenter: Score service is null.");
            return;
        }

        _service.ScoreChanged += OnScoreChanged;
        _service.KillChanged += OnKillChanged;

        // 초기 표시
        OnScoreChanged(_service.TotalScore);
        OnKillChanged(_service.TotalKills);
    }

    void OnDisable()
    {
        if (_service != null)
        {
            _service.ScoreChanged -= OnScoreChanged;
            _service.KillChanged -= OnKillChanged;
        }
    }

    void OnScoreChanged(float total)
    {
        if (scoreText)
            scoreText.text = $"Score: {total:0}";
    }

    void OnKillChanged(int total)
    {
        if (killsText)
            killsText.text = $"Kills: {total}";
    }
}
