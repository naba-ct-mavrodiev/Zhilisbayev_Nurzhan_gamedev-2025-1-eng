// ============================================
// Mouse Trigger
// ============================================
// PURPOSE: Fires events for mouse button and scroll actions
// USAGE: Attach to any GameObject
// EVENTS:
//   - Left/Right/Middle Down/Up events
//   - ScrollUp/ScrollDown events
// ============================================

using UnityEngine;
using UnityEngine.Events;

public class MouseTrigger : MonoBehaviour
{
    // ===== Events - Left Button =====
    [Header("Left Button (0)")]
    public UnityEvent LeftDownEvent;
    public UnityEvent LeftUpEvent;

    // ===== Events - Right Button =====
    [Header("Right Button (1)")]
    public UnityEvent RightDownEvent;
    public UnityEvent RightUpEvent;

    // ===== Events - Middle Button =====
    [Header("Middle Button (2)")]
    public UnityEvent MiddleDownEvent;
    public UnityEvent MiddleUpEvent;

    // ===== Events - Scroll =====
    [Header("Scroll Wheel")]
    public UnityEvent ScrollUpEvent;
    public UnityEvent ScrollDownEvent;

    // ===== Input Logic =====
    private void Update()
    {
        // Left Button
        if (Input.GetMouseButtonDown(0)) LeftDownEvent?.Invoke();
        if (Input.GetMouseButtonUp(0)) LeftUpEvent?.Invoke();

        // Right Button
        if (Input.GetMouseButtonDown(1)) RightDownEvent?.Invoke();
        if (Input.GetMouseButtonUp(1)) RightUpEvent?.Invoke();

        // Middle Button
        if (Input.GetMouseButtonDown(2)) MiddleDownEvent?.Invoke();
        if (Input.GetMouseButtonUp(2)) MiddleUpEvent?.Invoke();

        // Scroll
        float scroll = Input.mouseScrollDelta.y;
        if (scroll > 0) ScrollUpEvent?.Invoke();
        if (scroll < 0) ScrollDownEvent?.Invoke();
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject
// 2. Connect events to Actions in Inspector
// ============================================
