// ============================================
// Timer Trigger
// ============================================
// PURPOSE: Fires events based on time
// USAGE: Attach to any GameObject, configure duration and ticks
// EVENTS:
//   - ProgressEvent - fires every frame with progress (0 to 1)
//   - TickEvent - fires when one tick completes
//   - CompleteEvent - fires when all ticks complete
// ============================================

using UnityEngine;
using UnityEngine.Events;

public class TimerTrigger : MonoBehaviour
{
    // ===== Settings =====
    [Header("Settings")]
    [Tooltip("How long one tick lasts (in seconds)")]
    [Min(0.01f)]
    public float duration = 1f;

    [Tooltip("Number of ticks to run. Set to 0 for infinite.")]
    [Min(0)]
    public int ticks = 1;

    [Tooltip("Start the timer automatically on Start")]
    public bool autoStart = true;

    // ===== Events =====
    [Header("Events")]
    [Tooltip("Fires EVERY FRAME. Passes progress (0 to 1) within the current tick. Use for smooth animations.")]
    public UnityEvent<float> ProgressEvent;

    [Tooltip("Fires ONCE when a tick completes (every [duration] seconds).")]
    public UnityEvent TickEvent;

    [Tooltip("Fires ONCE when all ticks are done. Only fires if ticks > 0.")]
    public UnityEvent CompleteEvent;

    // ===== State =====
    [Header("State (Read Only)")]
    [Tooltip("Progress within current tick (0 to 1)")]
    [SerializeField] private float progress;

    [Tooltip("How many ticks have completed")]
    [SerializeField] private int currentTick;

    [SerializeField] private bool isRunning;
    [SerializeField] private bool isCompleted;

    // ===== Internal =====
    private float elapsedTime = 0f;

    // ===== Properties =====
    public float Progress => progress;
    public int CurrentTick => currentTick;
    public bool IsRunning => isRunning;
    public bool IsCompleted => isCompleted;

    // ===== Lifecycle =====
    private void Start()
    {
        if (autoStart) Run();
    }

    private void Update()
    {
        if (!isRunning || isCompleted) return;

        elapsedTime += Time.deltaTime;
        progress = Mathf.Clamp01(elapsedTime / duration);
        ProgressEvent?.Invoke(progress);

        if (elapsedTime >= duration)
        {
            elapsedTime = 0f;
            currentTick++;
            TickEvent?.Invoke();

            if (ticks > 0 && currentTick >= ticks)
            {
                isRunning = false;
                isCompleted = true;
                CompleteEvent?.Invoke();
            }
        }
    }

    // ===== Actions =====
    [ContextMenu("Run")]
    public void Run()
    {
        if (ticks == 0 || currentTick < ticks)
        {
            isRunning = true;
        }
    }

    [ContextMenu("Stop")]
    public void Stop()
    {
        isRunning = false;
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        isCompleted = false;
        elapsedTime = 0f;
        progress = 0f;
        currentTick = 0;
    }

    [ContextMenu("Clear And Run")]
    public void ClearAndRun()
    {
        Clear();
        Run();
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject
// 2. Set duration (how long one tick lasts in seconds)
// 3. Set ticks (0 = infinite, or number of ticks to run)
// 4. Connect events:
//    - ProgressEvent: for smooth animations (receives 0-1)
//    - TickEvent: for actions every [duration] seconds
//    - CompleteEvent: for actions when timer finishes
// ============================================
