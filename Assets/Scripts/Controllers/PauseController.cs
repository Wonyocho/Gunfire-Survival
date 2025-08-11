using UnityEngine;

public interface IPauseController
{
    bool IsPaused { get; }
}

/// <summary>
/// Entry point that wires input and service to control pausing. Attach to a scene GameObject.
/// </summary>
public class PauseController : MonoBehaviour, IPauseController
{
    [Tooltip("Optional: Automatically resume after seconds. 0 or negative means no auto-resume.")]
    [SerializeField] private float autoResumeSeconds = 0f;

    private IPauseInput _input;
    private IPauseService _service;
    private float _resumeAtRealtime = -1f;

    public bool IsPaused => _service?.IsPaused ?? false;

    private void Awake()
    {
        // Dependency creation. Could be replaced with DI container.
        _input = new KeyboardPauseInput();
        _service = new TimeScalePauseService();
    }

    private void Update()
    {
        if (_input.PausePressed)
        {
            Toggle();
        }

        if (autoResumeSeconds > 0f && _service.IsPaused && _resumeAtRealtime > 0f)
        {
            if (Time.unscaledTime >= _resumeAtRealtime)
            {
                Resume();
            }
        }
    }

    public void Toggle()
    {
        if (_service.IsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        _service.Pause();
        if (autoResumeSeconds > 0f)
        {
            _resumeAtRealtime = Time.unscaledTime + autoResumeSeconds;
        }
        else
        {
            _resumeAtRealtime = -1f;
        }
    }

    public void Resume()
    {
        _service.Resume();
        _resumeAtRealtime = -1f;
    }
}
