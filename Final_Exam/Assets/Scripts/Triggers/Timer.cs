// ============================================
// Timer
// ============================================
// PURPOSE: A reusable timer that fires events based on time
// USAGE: Used by TimerTrigger component
// ============================================

using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Timer
{
    // ===== Settings =====
    [Tooltip("Duration of one cycle in seconds")]
    [Min(0.01f)]
    public float duration = 1f;

    [Tooltip("How many cycles to run (0 = infinite)")]
    [Min(0)]
    public int count = 1;

    // ===== Events =====
    [Tooltip("Fires every frame with normalized progress (0 to 1)")]
    public UnityEvent<float> TickEvent;

    [Tooltip("Fires when duration is reached")]
    public UnityEvent TimeEvent;

    [Tooltip("Fires when all cycles complete")]
    public UnityEvent CompleteEvent;

    // ===== State =====
    private float elapsedTime = 0f;
    private int currentCount = 0;
    private bool isRunning = false;
    private bool isCompleted = false;

    // ===== Properties =====
    public float ElapsedTime => elapsedTime;
    public float Progress => Mathf.Clamp01(elapsedTime / duration);
    public int CurrentCount => currentCount;
    public bool IsRunning => isRunning;
    public bool IsCompleted => isCompleted;

    // ===== Methods =====
    public void Run()
    {
        if (count == 0 || currentCount < count)
        {
            isRunning = true;
        }
    }

    public void Stop()
    {
        isRunning = false;
    }

    public void Clear()
    {
        isCompleted = false;
        elapsedTime = 0f;
        currentCount = 0;
    }

    public void ClearAndRun()
    {
        Clear();
        Run();
    }

    public void Update(float deltaTime)
    {
        if (!isRunning || isCompleted) return;

        elapsedTime += deltaTime;
        TickEvent?.Invoke(Progress);

        if (elapsedTime >= duration)
        {
            elapsedTime = 0f;
            currentCount++;
            TimeEvent?.Invoke();

            if (count > 0 && currentCount >= count)
            {
                isRunning = false;
                isCompleted = true;
                CompleteEvent?.Invoke();
            }
        }
    }
}
