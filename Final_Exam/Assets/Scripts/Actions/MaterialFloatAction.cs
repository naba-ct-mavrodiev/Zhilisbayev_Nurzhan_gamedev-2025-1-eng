// ============================================
// Material Float Action
// ============================================
// PURPOSE: Sets a material's float property (one-shot or animated)
// USAGE: Attach to any GameObject, set target renderer and value
// ACTIONS:
//   - SetValue() - set value instantly or animated
// ============================================

using UnityEngine;
using DG.Tweening;

public class MaterialFloatAction : MonoBehaviour
{
    // ===== Target =====
    [Header("Target")]
    public Renderer targetRenderer;
    public int materialIndex = 0;

    // ===== Settings =====
    [Header("Settings")]
    public string propertyName = "_Metallic";
    public float targetValue = 1f;
    public float duration = 0f;

    // ===== Internal =====
    private Material materialInstance;
    private bool instanceCreated = false;

    // ===== Actions =====
    [ContextMenu("Set Value")]
    public void SetValue()
    {
        if (targetRenderer == null) return;

        EnsureMaterialInstance();

        if (materialInstance == null) return;
        if (!materialInstance.HasProperty(propertyName)) return;

        if (duration > 0)
        {
            materialInstance.DOFloat(targetValue, propertyName, duration);
        }
        else
        {
            materialInstance.SetFloat(propertyName, targetValue);
        }
    }

    public void SetValue(float customDuration)
    {
        if (targetRenderer == null) return;

        EnsureMaterialInstance();

        if (materialInstance == null) return;
        if (!materialInstance.HasProperty(propertyName)) return;

        materialInstance.DOFloat(targetValue, propertyName, customDuration);
    }

    public void SetValue(float value, float customDuration)
    {
        targetValue = value;
        SetValue(customDuration);
    }

    // ===== Internal =====
    private void EnsureMaterialInstance()
    {
        if (!instanceCreated)
        {
            materialInstance = targetRenderer.materials[materialIndex];
            instanceCreated = true;
        }
    }

    private void OnDestroy()
    {
        if (instanceCreated && materialInstance != null)
        {
            if (Application.isPlaying)
            {
                Destroy(materialInstance);
            }
        }
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject
// 2. Set targetRenderer
// 3. Set propertyName (e.g., "_Metallic", "_Smoothness", "_Cutoff")
// 4. Set targetValue
// 5. Set duration (0 = instant, >0 = animated)
// 6. Call SetValue() from triggers
// ============================================
