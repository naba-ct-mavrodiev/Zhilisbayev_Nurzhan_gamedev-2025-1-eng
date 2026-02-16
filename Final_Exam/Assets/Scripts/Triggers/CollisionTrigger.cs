// ============================================
// Collision Trigger (3D)
// ============================================
// PURPOSE: Fires events when objects physically collide
// USAGE: Attach to GameObject with Collider and Rigidbody
// EVENTS:
//   - EnterEvent - fires when collision starts
//   - ExitEvent - fires when collision ends
// ============================================

using UnityEngine;
using UnityEngine.Events;

public class CollisionTrigger : MonoBehaviour
{
    // ===== Settings =====
    [Header("Settings")]
    [Tooltip("Only trigger for objects with this tag (leave empty for any)")]
    public string filterTag = "";

    // ===== Events =====
    [Header("Events")]
    public UnityEvent EnterEvent;
    public UnityEvent ExitEvent;

    // ===== Collision Logic =====
    private void OnCollisionEnter(Collision collision)
    {
        if (string.IsNullOrEmpty(filterTag) || collision.gameObject.CompareTag(filterTag))
        {
            EnterEvent?.Invoke();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (string.IsNullOrEmpty(filterTag) || collision.gameObject.CompareTag(filterTag))
        {
            ExitEvent?.Invoke();
        }
    }

    // ===== Gizmos =====
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
        Gizmos.matrix = transform.localToWorldMatrix;

        BoxCollider box = GetComponent<BoxCollider>();
        if (box != null)
        {
            Gizmos.DrawCube(box.center, box.size);
            Gizmos.DrawWireCube(box.center, box.size);
            return;
        }

        SphereCollider sphere = GetComponent<SphereCollider>();
        if (sphere != null)
        {
            Gizmos.DrawSphere(sphere.center, sphere.radius);
            Gizmos.DrawWireSphere(sphere.center, sphere.radius);
            return;
        }

        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        if (capsule != null)
        {
            Gizmos.DrawWireSphere(capsule.center, capsule.radius);
        }
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to GameObject with Collider
// 2. Ensure one object has a Rigidbody
// 3. Set filterTag to limit which objects trigger (optional)
// 4. Connect events to Actions in Inspector
// ============================================
