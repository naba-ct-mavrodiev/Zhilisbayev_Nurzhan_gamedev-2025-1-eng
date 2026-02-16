// ============================================
// [Name] Trigger
// ============================================
// PURPOSE: [One sentence - what fires this trigger?]
// USAGE: [Where to attach it and what it needs]
// EVENTS:
//   - OnTriggered - fires when [condition]
// ============================================

using UnityEngine;
using UnityEngine.Events;

public class ExampleTrigger : MonoBehaviour
{
    // ===== Events =====
    [Header("Events")]
    public UnityEvent OnTriggered;

    // ===== Trigger Logic =====
    // Replace with your condition: OnTriggerEnter, Update, Input, etc.

    private void Fire()
    {
        OnTriggered?.Invoke();
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Duplicate this template and rename
// 2. Add your trigger condition (collision, timer, input)
// 3. Call Fire() when condition is met
// 4. Connect OnTriggered event to Actions in Inspector
// ============================================
