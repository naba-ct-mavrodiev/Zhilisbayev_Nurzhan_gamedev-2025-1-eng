// ============================================
// Instance Material Color Controller
// ============================================
// PURPOSE: Controls a color shader property on a material instance (affects only this object)
// USAGE: Attach to any GameObject with a Renderer
// NOTE: Can be animated through Unity's animation system
// ============================================

using UnityEngine;

public class InstanceMaterialColorController : MonoBehaviour
{
    // ===== Target =====
    [Header("Target")]
    public Renderer targetRenderer;
    public int materialIndex = 0;

    // ===== Parameters =====
    [Header("Property")]
    public string propertyName = "_BaseColor";

    [Header("Value")]
    [ColorUsage(true, true)]
    public Color propertyColor = Color.white;

    // ===== Cached =====
    private Material materialInstance;
    private Color lastColor = Color.clear;
    private bool instanceCreated = false;

    // ===== Lifecycle =====
    private void OnEnable()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        Initialize();
    }

    private void Update()
    {
        if (propertyColor != lastColor)
        {
            ApplyEffect();
            lastColor = propertyColor;
        }
    }

    private void OnDestroy()
    {
        if (instanceCreated && materialInstance != null)
        {
            if (Application.isPlaying)
                Destroy(materialInstance);
        }
    }

    // ===== Internal =====
    private void Initialize()
    {
        if (targetRenderer == null) return;
        if (materialIndex >= targetRenderer.sharedMaterials.Length) return;

        if (!instanceCreated)
        {
            materialInstance = targetRenderer.materials[materialIndex];
            instanceCreated = true;
        }

        ApplyEffect();
    }

    private void ApplyEffect()
    {
        if (materialInstance == null || string.IsNullOrEmpty(propertyName)) return;

        if (materialInstance.HasProperty(propertyName))
        {
            materialInstance.SetColor(propertyName, propertyColor);
        }
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to GameObject with Renderer
// 2. Set targetRenderer (or auto-finds from this GameObject)
// 3. Set propertyName (e.g., "_BaseColor", "_EmissionColor")
// 4. Adjust propertyColor in Inspector or animate it
// 5. Only affects THIS object (creates material instance)
// ============================================
