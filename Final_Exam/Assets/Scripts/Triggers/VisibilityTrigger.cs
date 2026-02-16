// ============================================
// Visibility Trigger
// ============================================
// PURPOSE: Fires events when object becomes visible/invisible to camera
// USAGE: Attach to GameObject with Renderer
// EVENTS:
//   - VisibleEvent - fires when object becomes visible
//   - InvisibleEvent - fires when object becomes invisible
// ============================================

using UnityEngine;
using UnityEngine.Events;

public class VisibilityTrigger : MonoBehaviour
{
    // ===== Events =====
    [Header("Events")]
    public UnityEvent VisibleEvent;
    public UnityEvent InvisibleEvent;

    // ===== Visibility Callbacks =====
    private void OnBecameVisible()
    {
        VisibleEvent?.Invoke();
    }

    private void OnBecameInvisible()
    {
        InvisibleEvent?.Invoke();
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to GameObject with Renderer component
// 2. Connect events to Actions in Inspector
// 3. Works with any camera including Cinemachine virtual cameras
// ============================================
