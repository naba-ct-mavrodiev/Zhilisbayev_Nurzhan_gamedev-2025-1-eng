// ============================================
// Instance Material Float Controller
// ============================================
// PURPOSE: Controls a float shader property on a material instance (affects only this object)
// USAGE: Attach to any GameObject with a Renderer
// NOTE: Can be animated through Unity's animation system
// ============================================

using UnityEngine;

public class InstanceMaterialController : MonoBehaviour
{
    // ===== Target =====
    [Header("Target")]
    public Renderer targetRenderer;
    public int materialIndex = 0;

    // ===== Parameters =====
    [Header("Property")]
    public string propertyName = "_Metallic";

    [Header("Value")]
    public float propertyValue = 0f;

    // ===== Cached =====
    private Material materialInstance;
    private float lastValue = float.MinValue;
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
        if (!Mathf.Approximately(propertyValue, lastValue))
        {
            ApplyEffect();
            lastValue = propertyValue;
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
            materialInstance.SetFloat(propertyName, propertyValue);
        }
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to GameObject with Renderer
// 2. Set targetRenderer (or auto-finds from this GameObject)
// 3. Set propertyName (e.g., "_Metallic", "_Smoothness")
// 4. Adjust propertyValue in Inspector or animate it
// 5. Only affects THIS object (creates material instance)
// ============================================
