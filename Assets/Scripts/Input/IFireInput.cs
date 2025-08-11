using UnityEngine;

public interface IFireInput
{
    bool IsFiring { get; }
    bool ReloadPressed { get; }
    bool SwitchSlotPressed { get; }
}

public class MouseKeyboardFireInput : IFireInput
{
    public bool IsFiring => Input.GetMouseButton(0);
    public bool ReloadPressed => Input.GetKeyDown(KeyCode.R);
    public bool SwitchSlotPressed => Input.GetKeyDown(KeyCode.Q);
}
