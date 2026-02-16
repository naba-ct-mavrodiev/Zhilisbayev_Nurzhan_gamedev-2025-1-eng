// ============================================
// Sphere Cast Trigger
// ============================================
// PURPOSE: Fires events when sphere cast hits or stops hitting something
// USAGE: Attach to any GameObject, sphere casts from transform forward
// EVENTS:
//   - HitEvent - fires when sphere hits something
//   - LostEvent - fires when sphere stops hitting
// ============================================

using UnityEngine;
using UnityEngine.Events;

public class SphereCastTrigger : MonoBehaviour
{
    // ===== Settings =====
    [Header("Settings")]
    public float radius = 0.5f;
    public float distance = 10f;
    public LayerMask layerMask = ~0;

    // ===== Events =====
    [Header("Events")]
    public UnityEvent HitEvent;
    public UnityEvent LostEvent;

    // ===== State =====
    private bool wasHitting = false;

    // ===== SphereCast Logic =====
    private void Update()
    {
        bool isHitting = Physics.SphereCast(transform.position, radius, transform.forward, out _, distance, layerMask);

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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * distance);
        Gizmos.DrawWireSphere(transform.position + transform.forward * distance, radius);
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject
// 2. Set radius, distance, and layerMask
// 3. Sphere shoots from object's position in forward direction
// 4. Connect events to Actions in Inspector
// ============================================
