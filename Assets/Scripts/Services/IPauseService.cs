public interface IPauseService
{
    bool IsPaused { get; }
    void Pause();
    void Resume();
}

public class TimeScalePauseService : IPauseService
{
    public bool IsPaused { get; private set; }

    public void Pause()
    {
        if (IsPaused) return;
        UnityEngine.Time.timeScale = 0f;
        IsPaused = true;
    }

    public void Resume()
    {
        if (!IsPaused) return;
        UnityEngine.Time.timeScale = 1f;
        IsPaused = false;
    }
}
