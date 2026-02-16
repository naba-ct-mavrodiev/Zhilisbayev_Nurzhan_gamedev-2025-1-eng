// ============================================
// Key Trigger (Old Input System)
// ============================================
// PURPOSE: Fires events when a keyboard key is pressed/released
// USAGE: Attach to any GameObject, select key in Inspector
// EVENTS:
//   - PressedEvent - fires when key is pressed down
//   - ReleasedEvent - fires when key is released
// ============================================

using UnityEngine;
using UnityEngine.Events;

public class KeyTrigger : MonoBehaviour
{
    // ===== Settings =====
    [Header("Settings")]
    public KeyCode key = KeyCode.Space;

    // ===== Events =====
    [Header("Events")]
    public UnityEvent PressedEvent;
    public UnityEvent ReleasedEvent;

    // ===== Input Logic =====
    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            PressedEvent?.Invoke();
        }

        if (Input.GetKeyUp(key))
        {
            ReleasedEvent?.Invoke();
        }
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject
// 2. Select key from dropdown in Inspector
// 3. Connect events to Actions in Inspector
// ============================================
