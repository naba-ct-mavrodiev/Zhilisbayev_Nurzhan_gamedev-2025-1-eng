// ============================================
// [Name] Action
// ============================================
// PURPOSE: [One sentence - what does this action do?]
// USAGE: [Where to attach it and what target it needs]
// ACTIONS:
//   - [MeaningfulName]() - what it does (e.g., Spawn, Destroy, Move, Shake)
// ============================================

using UnityEngine;
using DG.Tweening;

public class ExampleAction : MonoBehaviour
{
    // ===== Target =====
    [Header("Target")]
    public GameObject target;

    // ===== Settings =====
    [Header("Settings")]
    public float parameter1 = 1f;

    // ===== Actions =====
    // IMPORTANT: Use meaningful method names, NOT "Execute"
    // Good: Spawn(), Destroy(), Move(), Rotate(), Shake(), Explode()
    // Bad: Execute(), Run(), Do()

    [ContextMenu("Meaningful Name")]
    public void MeaningfulName()
    {
        if (target == null) return;

        // Your action here
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Duplicate this template and rename
// 2. Set the target type
// 3. Implement action with MEANINGFUL method name
// 4. Connect to Trigger events in Inspector
// ============================================
