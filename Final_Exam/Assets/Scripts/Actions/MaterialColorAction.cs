// ============================================
// Material Color Action
// ============================================
// PURPOSE: Sets a material's color property (one-shot or animated)
// USAGE: Attach to any GameObject, set target renderer and color
// ACTIONS:
//   - SetColor() - set color instantly or animated
// ============================================

using UnityEngine;
using DG.Tweening;

public class MaterialColorAction : MonoBehaviour
{
    // ===== Target =====
    [Header("Target")]
    public Renderer targetRenderer;
    public int materialIndex = 0;

    // ===== Settings =====
    [Header("Settings")]
    public string propertyName = "_BaseColor";
    public Color targetColor = Color.white;
    public float duration = 0f;

    // ===== Internal =====
    private Material materialInstance;
    private bool instanceCreated = false;

    // ===== Actions =====
    [ContextMenu("Set Color")]
    public void SetColor()
    {
        if (targetRenderer == null) return;

        EnsureMaterialInstance();

        if (materialInstance == null) return;
        if (!materialInstance.HasProperty(propertyName)) return;

        if (duration > 0)
        {
            materialInstance.DOColor(targetColor, propertyName, duration);
        }
        else
        {
            materialInstance.SetColor(propertyName, targetColor);
        }
    }

    public void SetColor(float customDuration)
    {
        if (targetRenderer == null) return;

        EnsureMaterialInstance();

        if (materialInstance == null) return;
        if (!materialInstance.HasProperty(propertyName)) return;

        materialInstance.DOColor(targetColor, propertyName, customDuration);
    }

    public void SetColor(Color color)
    {
        targetColor = color;
        SetColor();
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
// 3. Set propertyName (e.g., "_BaseColor", "_EmissionColor")
// 4. Set targetColor
// 5. Set duration (0 = instant, >0 = animated)
// 6. Call SetColor() from triggers
// ============================================
