// ============================================
// Animation Event Trigger
// ============================================
// PURPOSE: Bridge animation events to UnityEvents
// USAGE: Attach to GameObject with Animator, call from Animation Events
// EVENTS:
//   - Event1 through Event5 - connect to Actions in Inspector
// ============================================

using UnityEngine;
using UnityEngine.Events;

public class AnimationEventTrigger : MonoBehaviour
{
    // ===== Events =====
    [Header("Events")]
    public UnityEvent Event1;
    public UnityEvent Event2;
    public UnityEvent Event3;
    public UnityEvent Event4;
    public UnityEvent Event5;

    // ===== Animation Event Receivers =====
    public void FireEvent1() => Event1?.Invoke();
    public void FireEvent2() => Event2?.Invoke();
    public void FireEvent3() => Event3?.Invoke();
    public void FireEvent4() => Event4?.Invoke();
    public void FireEvent5() => Event5?.Invoke();
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to GameObject with Animator
// 2. In Animation window, add Animation Event at desired frame
// 3. Select FireEvent1, FireEvent2, etc. as the function
// 4. Connect Event1, Event2, etc. to Actions in Inspector
// ============================================
