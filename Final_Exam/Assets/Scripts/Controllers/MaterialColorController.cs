// ============================================
// Material Color Controller (Shared)
// ============================================
// PURPOSE: Controls a color shader property on a shared material (affects all objects)
// USAGE: Attach to any GameObject, set target material
// NOTE: Can be animated through Unity's animation system
// ============================================

using UnityEngine;

public class MaterialColorController : MonoBehaviour
{
    // ===== Target =====
    [Header("Target")]
    public Material targetMaterial;

    // ===== Parameters =====
    [Header("Property")]
    public string propertyName = "_BaseColor";

    [Header("Value")]
    [ColorUsage(true, true)]
    public Color propertyColor = Color.white;

    // ===== Cached =====
    private Color lastColor = Color.clear;

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
        if (propertyColor != lastColor)
        {
            ApplyEffect();
            lastColor = propertyColor;
        }
    }

    // ===== Internal =====
    private void ApplyEffect()
    {
        if (targetMaterial == null || string.IsNullOrEmpty(propertyName)) return;

        if (targetMaterial.HasProperty(propertyName))
        {
            targetMaterial.SetColor(propertyName, propertyColor);
        }
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject
// 2. Set targetMaterial (or auto-finds from Renderer)
// 3. Set propertyName (e.g., "_BaseColor", "_EmissionColor")
// 4. Adjust propertyColor in Inspector or animate it
// 5. WARNING: Affects ALL objects using this material
// ============================================
