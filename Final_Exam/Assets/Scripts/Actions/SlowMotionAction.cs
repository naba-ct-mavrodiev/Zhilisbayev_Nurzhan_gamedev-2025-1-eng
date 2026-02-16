// ============================================
// Slow Motion Action
// ============================================
// PURPOSE: Controls time scale for slow motion effects
// USAGE: Attach to any GameObject
// ACTIONS:
//   - SlowDown() - set time scale
//   - ResetTime() - restore normal speed
//   - Pause() / Unpause()
// ============================================

using UnityEngine;
using DG.Tweening;

public class SlowMotionAction : MonoBehaviour
{
    // ===== Settings =====
    [Header("Settings")]
    [Range(0f, 2f)]
    public float targetTimeScale = 0.2f;
    public float duration = 0f;

    [Header("Auto Reset")]
    public bool autoReset = false;
    public float autoResetDelay = 1f;

    // ===== State =====
    private float originalFixedDeltaTime;

    private void Awake()
    {
        originalFixedDeltaTime = Time.fixedDeltaTime;
    }

    // ===== Actions =====
    [ContextMenu("Slow Down")]
    public void SlowDown()
    {
        if (duration > 0)
        {
            DOTween.To(() => Time.timeScale, x => SetTimeScale(x), targetTimeScale, duration)
                .SetUpdate(true);
        }
        else
        {
            SetTimeScale(targetTimeScale);
        }

        if (autoReset)
        {
            Invoke(nameof(ResetTime), autoResetDelay * targetTimeScale);
        }
    }

    public void SlowDown(float customTimeScale)
    {
        targetTimeScale = customTimeScale;
        SlowDown();
    }

    [ContextMenu("Reset Time")]
    public void ResetTime()
    {
        if (duration > 0)
        {
            DOTween.To(() => Time.timeScale, x => SetTimeScale(x), 1f, duration)
                .SetUpdate(true);
        }
        else
        {
            SetTimeScale(1f);
        }
    }

    [ContextMenu("Pause")]
    public void Pause()
    {
        SetTimeScale(0f);
    }

    [ContextMenu("Unpause")]
    public void Unpause()
    {
        SetTimeScale(1f);
    }

    // ===== Internal =====
    private void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = originalFixedDeltaTime * scale;
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject
// 2. Set targetTimeScale (0.2 = 20% speed, 0.5 = half speed)
// 3. Set duration for smooth transition (0 = instant)
// 4. Enable autoReset to return to normal after delay
// 5. Call SlowDown() from triggers
// ============================================
