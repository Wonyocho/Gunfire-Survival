using UnityEngine;

public interface IPauseInput
{
    bool PausePressed { get; }
}

public class KeyboardPauseInput : IPauseInput
{
    public bool PausePressed => Input.GetKeyDown(KeyCode.Escape);
}
