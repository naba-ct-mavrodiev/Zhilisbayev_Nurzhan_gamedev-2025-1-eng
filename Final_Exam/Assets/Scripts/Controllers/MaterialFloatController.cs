// ============================================
// Material Float Controller (Shared)
// ============================================
// PURPOSE: Controls a float shader property on a shared material (affects all objects)
// USAGE: Attach to any GameObject, set target material
// NOTE: Can be animated through Unity's animation system
// ============================================

using UnityEngine;

public class MaterialFloatController : MonoBehaviour
{
    // ===== Target =====
    [Header("Target")]
    public Material targetMaterial;

    // ===== Parameters =====
    [Header("Property")]
    public string propertyName = "_Metallic";

    [Header("Value")]
    public float propertyValue = 0f;

    // ===== Cached =====
    private float lastValue = float.MinValue;

    // ===== Lifecycle =====
    private void OnEnable()
    {
        if (targetMaterial == null)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
                targetMaterial = renderer.sharedMaterial;
        }

        ApplyEffect();
    }

    private void Update()
    {
        if (!Mathf.Approximately(propertyValue, lastValue))
        {
            ApplyEffect();
            lastValue = propertyValue;
        }
    }

    // ===== Internal =====
    private void ApplyEffect()
    {
        if (targetMaterial == null || string.IsNullOrEmpty(propertyName)) return;

        if (targetMaterial.HasProperty(propertyName))
        {
            targetMaterial.SetFloat(propertyName, propertyValue);
        }
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject
// 2. Set targetMaterial (or auto-finds from Renderer)
// 3. Set propertyName (e.g., "_Metallic", "_Smoothness")
// 4. Adjust propertyValue in Inspector or animate it
// 5. WARNING: Affects ALL objects using this material
// ============================================
