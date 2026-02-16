// ============================================
// Force Action
// ============================================
// PURPOSE: Adds force to a Rigidbody
// USAGE: Attach to any GameObject, set target Rigidbody
// ACTIONS:
//   - Push() - add force
// ============================================

using UnityEngine;

public class ForceAction : MonoBehaviour
{
    // ===== Target =====
    [Header("Target")]
    public Rigidbody target;

    // ===== Settings =====
    [Header("Settings")]
    public Vector3 force = Vector3.up * 10f;
    public ForceMode forceMode = ForceMode.Impulse;
    public bool useLocalDirection = false;

    // ===== Actions =====
    [ContextMenu("Push")]
    public void Push()
    {
        if (target == null) return;

        Vector3 finalForce = useLocalDirection ? target.transform.TransformDirection(force) : force;
        target.AddForce(finalForce, forceMode);
    }

    public void Push(float multiplier)
    {
        if (target == null) return;

        Vector3 finalForce = useLocalDirection ? target.transform.TransformDirection(force) : force;
        target.AddForce(finalForce * multiplier, forceMode);
    }

    // ===== Gizmos =====
    private void OnDrawGizmosSelected()
    {
        if (target == null) return;
        Gizmos.color = Color.blue;
        Vector3 start = target.position;
        Vector3 dir = useLocalDirection ? target.transform.TransformDirection(force.normalized) : force.normalized;
        Gizmos.DrawRay(start, dir * 2f);
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject
// 2. Set target Rigidbody
// 3. Set force direction and magnitude
// 4. Choose ForceMode (Impulse for instant, Force for continuous)
// 5. Call Push() from triggers
// ============================================
