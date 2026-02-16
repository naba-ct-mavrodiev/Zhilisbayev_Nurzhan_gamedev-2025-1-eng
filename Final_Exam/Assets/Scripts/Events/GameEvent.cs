// ============================================
// Game Event (ScriptableObject)
// ============================================
// PURPOSE: A simple event that can be raised and listened to across objects
// USAGE: Create via Assets > Create > Events > Game Event
// ACTIONS:
//   - Raise() - fires the event to all listeners
// ============================================

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Game Event", fileName = "New Game Event")]
public class GameEvent : ScriptableObject
{
    // ===== Listeners =====
    private List<GameEventListener> listeners = new List<GameEventListener>();

    // ===== Actions =====
    [ContextMenu("Raise")]
    public void Raise()
    {
        // Notify all listeners (iterate backwards for safe removal)
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    // ===== Registration =====
    public void Register(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void Unregister(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Right-click in Project > Create > Events > Game Event
// 2. Name your event (e.g., "OnPlayerDied", "OnDoorOpened")
// 3. Reference this event in GameEventRaiser and GameEventListener
// ============================================
