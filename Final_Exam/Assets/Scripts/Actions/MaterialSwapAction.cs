// ============================================
// Material Swap Action
// ============================================
// PURPOSE: Swaps a renderer's material to a different one
// USAGE: Attach to any GameObject, set target renderer and new material
// ACTIONS:
//   - Swap() - swap to new material
//   - Revert() - swap back to original
// ============================================

using UnityEngine;

public class MaterialSwapAction : MonoBehaviour
{
    // ===== Target =====
    [Header("Target")]
    public Renderer targetRenderer;
    public int materialIndex = 0;

    // ===== Settings =====
    [Header("Settings")]
    public Material newMaterial;

    // ===== State =====
    private Material originalMaterial;
    private bool hasOriginal = false;

    // ===== Actions =====
    [ContextMenu("Swap")]
    public void Swap()
    {
        if (targetRenderer == null || newMaterial == null) return;

        if (!hasOriginal)
        {
            originalMaterial = targetRenderer.sharedMaterials[materialIndex];
            hasOriginal = true;
        }

        Material[] materials = targetRenderer.materials;
        materials[materialIndex] = newMaterial;
        targetRenderer.materials = materials;
    }

    [ContextMenu("Revert")]
    public void Revert()
    {
        if (targetRenderer == null || !hasOriginal) return;

        Material[] materials = targetRenderer.materials;
        materials[materialIndex] = originalMaterial;
        targetRenderer.materials = materials;
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject
// 2. Set targetRenderer
// 3. Set newMaterial to swap to
// 4. Call Swap() from triggers
// 5. Call Revert() to restore original
// ============================================
