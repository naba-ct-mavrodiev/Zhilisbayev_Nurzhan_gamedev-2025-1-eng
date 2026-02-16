// ============================================
// Look At Trigger
// ============================================
// PURPOSE: Fires events when camera/object looks at or away from this object
// USAGE: Attach to target GameObject
// EVENTS:
//   - LookingEvent - fires when looked at
//   - NotLookingEvent - fires when look away
// ============================================

using UnityEngine;
using UnityEngine.Events;

public class LookAtTrigger : MonoBehaviour
{
    // ===== Settings =====
    [Header("Settings")]
    [Tooltip("The viewer (leave empty for main camera)")]
    public Transform viewer;

    [Tooltip("How directly must viewer look at target (0 = 90°, 1 = exact)")]
    [Range(0f, 1f)]
    public float threshold = 0.9f;

    [Tooltip("Max distance to detect (0 = infinite)")]
    public float maxDistance = 0f;

    // ===== Events =====
    [Header("Events")]
    public UnityEvent LookingEvent;
    public UnityEvent NotLookingEvent;

    // ===== State =====
    private bool wasLooking = false;

    // ===== Look Detection =====
    private void Update()
    {
        Transform viewerTransform = viewer != null ? viewer : Camera.main.transform;
        if (viewerTransform == null) return;

        Vector3 directionToTarget = (transform.position - viewerTransform.position).normalized;
        float dot = Vector3.Dot(viewerTransform.forward, directionToTarget);

        bool withinDistance = maxDistance <= 0 || Vector3.Distance(viewerTransform.position, transform.position) <= maxDistance;
        bool isLooking = dot >= threshold && withinDistance;

        if (isLooking && !wasLooking)
        {
            LookingEvent?.Invoke();
        }
        else if (!isLooking && wasLooking)
        {
            NotLookingEvent?.Invoke();
        }

        wasLooking = isLooking;
    }

    // ===== Gizmos =====
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (maxDistance > 0)
        {
            Gizmos.DrawWireSphere(transform.position, maxDistance);
        }
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to the target object (what you look at)
// 2. Leave viewer empty to use main camera, or assign specific transform
// 3. Adjust threshold (higher = more precise look required)
// 4. Connect events to Actions in Inspector
// ============================================
