using UnityEngine;
using TMPro;

public class SurvivalTimer : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI survivalTimeText; // TextMeshPro UI 컴포넌트
    
    [Header("Settings")]
    [SerializeField] private string timeFormat = "Survival Time: {0:F1}s"; // 표시 형식
    [SerializeField] private bool showMinutesAndSeconds = true; // 분:초 형식으로 표시할지 여부
    
    private float survivalTime = 0f;
    private bool isGameActive = true;
    
    public float SurvivalTime => survivalTime;
    
    void Start()
    {
        // 게임 시작 시간 초기화
        survivalTime = 0f;
        isGameActive = true;
        
        // UI Text가 할당되지 않았다면 자동으로 찾기
        if (survivalTimeText == null)
        {
            survivalTimeText = GetComponent<TextMeshProUGUI>();
            if (survivalTimeText == null)
            {
                Debug.LogError("SurvivalTimer: TextMeshProUGUI component not found! Please assign survivalTimeText in the inspector.");
            }
        }
        
        UpdateTimeDisplay();
    }
    
    void Update()
    {
        if (isGameActive)
        {
            survivalTime += Time.deltaTime;
            UpdateTimeDisplay();
        }
    }
    
    void UpdateTimeDisplay()
    {
        if (survivalTimeText == null) return;
        
        string displayText;
        
        if (showMinutesAndSeconds)
        {
            int minutes = Mathf.FloorToInt(survivalTime / 60f);
            int seconds = Mathf.FloorToInt(survivalTime % 60f);
            int milliseconds = Mathf.FloorToInt((survivalTime * 10f) % 10f);
            
            displayText = $"{minutes:00}:{seconds:00}.{milliseconds}";
        }
        else
        {
            displayText = string.Format(timeFormat, survivalTime);
        }
        
        survivalTimeText.text = displayText;
    }
    
    /// <summary>
    /// 게임이 종료되었을 때 호출 (타이머 정지)
    /// </summary>
    public void StopTimer()
    {
        isGameActive = false;
        Debug.Log($"Game Over! Final Survival Time: {GetFormattedTime()}");
    }
    
    /// <summary>
    /// 게임을 재시작할 때 호출 (타이머 초기화)
    /// </summary>
    public void ResetTimer()
    {
        survivalTime = 0f;
        isGameActive = true;
        UpdateTimeDisplay();
    }
    
    /// <summary>
    /// 포맷된 시간 문자열 반환
    /// </summary>
    public string GetFormattedTime()
    {
        if (showMinutesAndSeconds)
        {
            int minutes = Mathf.FloorToInt(survivalTime / 60f);
            int seconds = Mathf.FloorToInt(survivalTime % 60f);
            int milliseconds = Mathf.FloorToInt((survivalTime * 10f) % 10f);
            return $"{minutes:00}:{seconds:00}.{milliseconds}";
        }
        else
        {
            return string.Format(timeFormat, survivalTime);
        }
    }
    
    /// <summary>
    /// 게임 일시정지/재개
    /// </summary>
    public void SetGameActive(bool active)
    {
        isGameActive = active;
    }
}
