using UnityEngine;

[CreateAssetMenu(fileName = "ShotLineVFXSettings", menuName = "VFX/Shot Line Settings", order = 0)]
public class ShotLineVFXSettings : ScriptableObject
{
    [Header("VFX Settings")]
    public float lineDuration = 0.05f;

    [Header("Line length by damage")]
    public float lineBaseLength = 3f;
    public float lineLengthPerDamage = 0.25f;
    public float minVisualLength = 0f;
    public bool clampToHitPoint = true;
}
