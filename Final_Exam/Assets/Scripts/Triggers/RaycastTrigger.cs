// ============================================
// Raycast Trigger
// ============================================
// PURPOSE: Fires events when ray hits or stops hitting something
// USAGE: Attach to any GameObject, ray casts from transform forward
// EVENTS:
//   - HitEvent - fires when ray hits something
//   - LostEvent - fires when ray stops hitting
// ============================================

using UnityEngine;
using UnityEngine.Events;

public class RaycastTrigger : MonoBehaviour
{
    // ===== Settings =====
    [Header("Settings")]
    public float distance = 10f;
    public LayerMask layerMask = ~0;

    // ===== Events =====
    [Header("Events")]
    public UnityEvent HitEvent;
    public UnityEvent LostEvent;

    // ===== State =====
    private bool wasHitting = false;

    // ===== Raycast Logic =====
    private void Update()
    {
        bool isHitting = Physics.Raycast(transform.position, transform.forward, distance, layerMask);

        if (isHitting && !wasHitting)
        {
            HitEvent?.Invoke();
        }
        else if (!isHitting && wasHitting)
        {
            LostEvent?.Invoke();
        }

        wasHitting = isHitting;
    }

    // ===== Gizmos =====
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * distance);
        Gizmos.DrawSphere(transform.position, 0.05f);
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject
// 2. Set distance and layerMask
// 3. Ray shoots from object's position in forward direction
// 4. Connect events to Actions in Inspector
// ============================================
