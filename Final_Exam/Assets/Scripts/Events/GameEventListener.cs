// ============================================
// Game Event Listener
// ============================================
// PURPOSE: Listens for a GameEvent and invokes a response
// USAGE: Attach to any GameObject that should react to an event
// EVENTS:
//   - Response - fires when the GameEvent is raised
// ============================================

using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    // ===== Event =====
    [Header("Event")]
    public GameEvent gameEvent;

    // ===== Response =====
    [Header("Response")]
    public UnityEvent Response;

    // ===== Lifecycle =====
    private void OnEnable()
    {
        if (gameEvent != null)
            gameEvent.Register(this);
    }

    private void OnDisable()
    {
        if (gameEvent != null)
            gameEvent.Unregister(this);
    }

    // ===== Callback =====
    public void OnEventRaised()
    {
        Response?.Invoke();
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject that should react
// 2. Assign the same GameEvent asset as the Raiser
// 3. Wire Response to any Actions (e.g., DestroyAction.Destroy)
// ============================================
