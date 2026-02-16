// ============================================
// Destroy Action
// ============================================
// PURPOSE: Destroys a target GameObject
// USAGE: Attach to any GameObject, set target
// ACTIONS:
//   - Destroy() - destroy target
//   - DestroySelf() - destroy this GameObject
// ============================================

using UnityEngine;

public class DestroyAction : MonoBehaviour
{
    // ===== Target =====
    [Header("Target")]
    public GameObject target;

    // ===== Settings =====
    [Header("Settings")]
    public float delay = 0f;

    // ===== Actions =====
    [ContextMenu("Destroy")]
    public void Destroy()
    {
        if (target == null) return;

        if (delay > 0)
        {
            Destroy(target, delay);
        }
        else
        {
            Destroy(target);
        }
    }

    public void Destroy(float customDelay)
    {
        if (target == null) return;
        Destroy(target, customDelay);
    }

    [ContextMenu("Destroy Self")]
    public void DestroySelf()
    {
        if (delay > 0)
        {
            Destroy(gameObject, delay);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject
// 2. Set target (or use DestroySelf)
// 3. Set delay if needed
// 4. Call Destroy() from triggers
// ============================================
