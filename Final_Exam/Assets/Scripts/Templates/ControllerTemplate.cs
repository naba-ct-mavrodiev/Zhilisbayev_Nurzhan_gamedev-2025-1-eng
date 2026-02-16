// ============================================
// [Name] Controller
// ============================================
// PURPOSE: [One sentence - what system does this control?]
// USAGE: [Where to attach it and what it needs]
// ACTIONS:
//   - ActionName() - what it does
//   - ActionName(duration) - animated version
// ============================================

using UnityEngine;
using DG.Tweening;

public class ExampleController : MonoBehaviour
{
    // ===== Target =====
    [Header("Target")]
    public Component targetComponent;

    // ===== Parameters =====
    [Header("Parameters")]
    [Range(0f, 1f)]
    public float parameter1 = 1f;

    // ===== Actions - One Shot =====
    [ContextMenu("Execute")]
    public void SetParameter1(float value)
    {
        parameter1 = value;
        ApplyEffect();
    }

    // ===== Actions - Duration =====
    public void AnimateParameter1(float targetValue, float duration)
    {
        DOTween.To(() => parameter1, x => parameter1 = x, targetValue, duration)
            .OnUpdate(() => ApplyEffect());
    }

    // ===== Internal =====
    private void ApplyEffect()
    {
        // Apply current parameter values to the target
        // Example:
        // targetMaterial.SetFloat("_Property", parameter1);
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to GameObject with [required component]
// 2. Set target reference in Inspector
// 3. Configure default parameters
// 4. Call actions from Triggers, Timeline, or Animation Events
// ============================================
