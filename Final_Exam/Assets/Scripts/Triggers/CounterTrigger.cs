// ============================================
// Counter Trigger
// ============================================
// PURPOSE: Fires event after being called X times
// USAGE: Attach to any GameObject, call Increment() from other events
// EVENTS:
//   - CountReachedEvent - fires when count reaches target
//   - IncrementEvent - fires on each increment
// ============================================

using UnityEngine;
using UnityEngine.Events;

public class CounterTrigger : MonoBehaviour
{
    // ===== Settings =====
    [Header("Settings")]
    [Tooltip("Number of calls needed to trigger")]
    public int targetCount = 5;

    [Tooltip("Reset counter after reaching target")]
    public bool autoReset = false;

    // ===== Events =====
    [Header("Events")]
    public UnityEvent CountReachedEvent;
    public UnityEvent IncrementEvent;

    // ===== State =====
    [Header("State")]
    [SerializeField] private int currentCount = 0;

    // ===== Properties =====
    public int CurrentCount => currentCount;

    // ===== Actions =====
    [ContextMenu("Increment")]
    public void Increment()
    {
        currentCount++;
        IncrementEvent?.Invoke();

        if (currentCount >= targetCount)
        {
            CountReachedEvent?.Invoke();

            if (autoReset)
            {
                currentCount = 0;
            }
        }
    }

    [ContextMenu("Reset")]
    public void Reset()
    {
        currentCount = 0;
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject
// 2. Set targetCount (e.g., 5 for "collect 5 items")
// 3. Call Increment() from other triggers/events
// 4. Connect CountReachedEvent to Actions in Inspector
// ============================================
