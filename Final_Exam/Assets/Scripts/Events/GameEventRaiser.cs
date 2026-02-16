// ============================================
// Game Event Raiser
// ============================================
// PURPOSE: Raises a GameEvent when triggered
// USAGE: Attach to any GameObject, wire Raise() to a UnityEvent
// ACTIONS:
//   - Raise() - fires the assigned GameEvent
// ============================================

using UnityEngine;

public class GameEventRaiser : MonoBehaviour
{
    // ===== Event =====
    [Header("Event")]
    public GameEvent gameEvent;

    // ===== Actions =====
    [ContextMenu("Raise")]
    public void Raise()
    {
        if (gameEvent == null) return;
        gameEvent.Raise();
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject
// 2. Assign a GameEvent asset
// 3. Wire Raise() to any UnityEvent trigger
//    (e.g., LifecycleTrigger.StartEvent, CollisionTrigger.OnEnter)
// ============================================
