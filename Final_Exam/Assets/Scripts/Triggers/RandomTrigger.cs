// ============================================
// Random Trigger
// ============================================
// PURPOSE: Fires event with a percentage chance when called
// USAGE: Attach to any GameObject, call TryFire() from other events
// EVENTS:
//   - SuccessEvent - fires when random check passes
//   - FailEvent - fires when random check fails
// ============================================

using UnityEngine;
using UnityEngine.Events;

public class RandomTrigger : MonoBehaviour
{
    // ===== Settings =====
    [Header("Settings")]
    [Tooltip("Chance to fire (0-100%)")]
    [Range(0f, 100f)]
    public float chance = 50f;

    // ===== Events =====
    [Header("Events")]
    public UnityEvent SuccessEvent;
    public UnityEvent FailEvent;

    // ===== Actions =====
    [ContextMenu("Try Fire")]
    public void TryFire()
    {
        float roll = Random.Range(0f, 100f);

        if (roll <= chance)
        {
            SuccessEvent?.Invoke();
        }
        else
        {
            FailEvent?.Invoke();
        }
    }

    [ContextMenu("Force Success")]
    public void ForceSuccess()
    {
        SuccessEvent?.Invoke();
    }

    [ContextMenu("Force Fail")]
    public void ForceFail()
    {
        FailEvent?.Invoke();
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject
// 2. Set chance (0-100%)
// 3. Call TryFire() from other triggers/events
// 4. Connect SuccessEvent/FailEvent to Actions in Inspector
// ============================================
