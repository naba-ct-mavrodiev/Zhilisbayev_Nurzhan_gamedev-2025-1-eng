// ============================================
// Lifecycle Trigger
// ============================================
// PURPOSE: Fires events at key moments in the GameObject's life
// USAGE: Attach to any GameObject
// EVENTS:
//   - AwakeEvent - fires first, before Start
//   - StartEvent - fires once when scene begins
//   - EnableEvent - fires when object is activated
//   - UpdateEvent - fires every frame
//   - DisableEvent - fires when object is deactivated
//   - DestroyEvent - fires when object is destroyed
// ============================================

using UnityEngine;
using UnityEngine.Events;

public class LifecycleTrigger : MonoBehaviour
{
    // ===== Events =====
    [Header("Events")]
    public UnityEvent AwakeEvent;
    public UnityEvent StartEvent;
    public UnityEvent EnableEvent;
    public UnityEvent UpdateEvent;
    public UnityEvent DisableEvent;
    public UnityEvent DestroyEvent;

    // ===== Lifecycle =====
    private void Awake()
    {
        AwakeEvent?.Invoke();
    }

    private void Start()
    {
        StartEvent?.Invoke();
    }

    private void OnEnable()
    {
        EnableEvent?.Invoke();
    }

    private void Update()
    {
        UpdateEvent?.Invoke();
    }

    private void OnDisable()
    {
        DisableEvent?.Invoke();
    }

    private void OnDestroy()
    {
        DestroyEvent?.Invoke();
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject
// 2. Connect events to Actions in Inspector
// ============================================
