// ============================================
// Rotate Action
// ============================================
// PURPOSE: Rotates target to a rotation
// USAGE: Attach to any GameObject, set target and rotation
// ACTIONS:
//   - Rotate() - rotate instantly or animated
// ============================================

using UnityEngine;
using DG.Tweening;

public class RotateAction : MonoBehaviour
{
    // ===== Target =====
    [Header("Target")]
    public Transform target;

    // ===== Settings =====
    [Header("Settings")]
    public Vector3 targetRotation;
    public float duration = 0f;
    public bool useLocalRotation = true;
    public bool additive = false;

    // ===== Lifecycle =====
    private void Awake()
    {
        if (target == null)
            target = transform;
    }

    // ===== Actions =====
    [ContextMenu("Rotate")]
    public void Rotate()
    {
        if (target == null) return;

        Vector3 finalRotation = additive ? GetCurrentRotation() + targetRotation : targetRotation;

        if (duration > 0)
        {
            if (useLocalRotation)
            {
                target.DOLocalRotate(finalRotation, duration);
            }
            else
            {
                target.DORotate(finalRotation, duration);
            }
        }
        else
        {
            if (useLocalRotation)
            {
                target.localEulerAngles = finalRotation;
            }
            else
            {
                target.eulerAngles = finalRotation;
            }
        }
    }

    public void Rotate(float customDuration)
    {
        if (target == null) return;

        Vector3 finalRotation = additive ? GetCurrentRotation() + targetRotation : targetRotation;

        if (useLocalRotation)
        {
            target.DOLocalRotate(finalRotation, customDuration);
        }
        else
        {
            target.DORotate(finalRotation, customDuration);
        }
    }

    private Vector3 GetCurrentRotation()
    {
        return useLocalRotation ? target.localEulerAngles : target.eulerAngles;
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject
// 2. Set target (what rotates) - defaults to this GameObject
// 3. Set targetRotation (euler angles)
// 4. Set duration (0 = instant, >0 = animated)
// 5. Enable additive to add to current rotation
// 6. Call Rotate() from triggers
// ============================================
