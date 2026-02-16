// ============================================
// Look At Action
// ============================================
// PURPOSE: Rotates target to look at another transform
// USAGE: Attach to any GameObject, set target and look target
// ACTIONS:
//   - LookAt() - look at instantly or animated
//   - StartLooking() / StopLooking() - continuous
// ============================================

using UnityEngine;
using DG.Tweening;

public class LookAtAction : MonoBehaviour
{
    // ===== Target =====
    [Header("Target")]
    public Transform target;
    public Transform lookTarget;

    // ===== Settings =====
    [Header("Settings")]
    public float duration = 0f;
    public float smoothSpeed = 5f;
    public bool lockX = false;
    public bool lockY = false;
    public bool lockZ = false;

    // ===== State =====
    private bool isLooking = false;

    // ===== Lifecycle =====
    private void Awake()
    {
        if (target == null)
            target = transform;
    }

    private void Update()
    {
        if (!isLooking || target == null || lookTarget == null) return;

        Vector3 direction = lookTarget.position - target.position;
        if (lockX) direction.x = 0;
        if (lockY) direction.y = 0;
        if (lockZ) direction.z = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            target.rotation = Quaternion.Lerp(target.rotation, targetRotation, smoothSpeed * Time.deltaTime);
        }
    }

    // ===== Actions =====
    [ContextMenu("Look At")]
    public void LookAt()
    {
        if (target == null || lookTarget == null) return;

        Vector3 direction = lookTarget.position - target.position;
        if (lockX) direction.x = 0;
        if (lockY) direction.y = 0;
        if (lockZ) direction.z = 0;

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        if (duration > 0)
        {
            target.DORotateQuaternion(targetRotation, duration);
        }
        else
        {
            target.rotation = targetRotation;
        }
    }

    [ContextMenu("Start Looking")]
    public void StartLooking()
    {
        isLooking = true;
    }

    [ContextMenu("Stop Looking")]
    public void StopLooking()
    {
        isLooking = false;
    }

    // ===== Gizmos =====
    private void OnDrawGizmosSelected()
    {
        if (target == null || lookTarget == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(target.position, lookTarget.position);
        Gizmos.DrawSphere(lookTarget.position, 0.1f);
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject
// 2. Set target (what rotates) - defaults to this GameObject
// 3. Set lookTarget (what to look at)
// 4. Use LookAt() for one-time, or StartLooking() for continuous
// 5. Lock axes to constrain rotation
// ============================================
