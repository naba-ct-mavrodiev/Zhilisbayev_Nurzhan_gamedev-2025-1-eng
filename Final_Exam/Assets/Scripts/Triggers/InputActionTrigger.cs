// ============================================
// Input Action Trigger (New Input System)
// ============================================
// PURPOSE: Fires events when an InputAction is performed/canceled
// USAGE: Attach to any GameObject, assign InputActionReference
// EVENTS:
//   - PerformedEvent - fires when action is performed
//   - CanceledEvent - fires when action is canceled
// ============================================

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputActionTrigger : MonoBehaviour
{
    // ===== Settings =====
    [Header("Settings")]
    public InputActionReference inputAction;

    // ===== Events =====
    [Header("Events")]
    public UnityEvent PerformedEvent;
    public UnityEvent CanceledEvent;

    // ===== Lifecycle =====
    private void OnEnable()
    {
        if (inputAction != null)
        {
            inputAction.action.Enable();
            inputAction.action.performed += OnPerformed;
            inputAction.action.canceled += OnCanceled;
        }
    }

    private void OnDisable()
    {
        if (inputAction != null)
        {
            inputAction.action.performed -= OnPerformed;
            inputAction.action.canceled -= OnCanceled;
        }
    }

    // ===== Input Logic =====
    private void OnPerformed(InputAction.CallbackContext context)
    {
        PerformedEvent?.Invoke();
    }

    private void OnCanceled(InputAction.CallbackContext context)
    {
        CanceledEvent?.Invoke();
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject
// 2. Create InputActionAsset or use existing one
// 3. Drag InputAction reference to inputAction field
// 4. Connect events to Actions in Inspector
// ============================================
