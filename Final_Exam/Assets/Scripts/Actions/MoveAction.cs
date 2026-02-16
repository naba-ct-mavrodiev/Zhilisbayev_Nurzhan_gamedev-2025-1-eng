// ============================================
// Move Action
// ============================================
// PURPOSE: Moves target to a position
// USAGE: Attach to any GameObject, set target and destination
// ACTIONS:
//   - Move() - move instantly or animated
// ============================================

using UnityEngine;
using DG.Tweening;

public class MoveAction : MonoBehaviour
{
    // ===== Target =====
    [Header("Target")]
    public Transform target;

    // ===== Settings =====
    [Header("Settings")]
    public Transform destination;
    public Vector3 offset;
    public float duration = 0f;
    public bool useLocalPosition = false;

    // ===== Lifecycle =====
    private void Awake()
    {
        if (target == null)
            target = transform;
    }

    // ===== Actions =====
    [ContextMenu("Move")]
    public void Move()
    {
        if (target == null || destination == null) return;

        Vector3 targetPosition = destination.position + offset;

        if (duration > 0)
        {
            if (useLocalPosition)
            {
                target.DOLocalMove(target.parent.InverseTransformPoint(targetPosition), duration);
            }
            else
            {
                target.DOMove(targetPosition, duration);
            }
        }
        else
        {
            if (useLocalPosition)
            {
                target.localPosition = target.parent.InverseTransformPoint(targetPosition);
            }
            else
            {
                target.position = targetPosition;
            }
        }
    }

    public void Move(float customDuration)
    {
        if (target == null || destination == null) return;

        Vector3 targetPosition = destination.position + offset;

        if (useLocalPosition)
        {
            target.DOLocalMove(target.parent.InverseTransformPoint(targetPosition), customDuration);
        }
        else
        {
            target.DOMove(targetPosition, customDuration);
        }
    }

    // ===== Gizmos =====
    private void OnDrawGizmosSelected()
    {
        if (destination == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(destination.position + offset, 0.1f);
        if (target != null)
        {
            Gizmos.DrawLine(target.position, destination.position + offset);
        }
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject
// 2. Set target (what moves) - defaults to this GameObject
// 3. Set destination (where it goes)
// 4. Set duration (0 = instant, >0 = animated)
// 5. Call Move() from triggers
// ============================================
