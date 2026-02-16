// ============================================
// Environment Preset (ScriptableObject)
// ============================================
// PURPOSE: Stores a complete environment configuration
// USAGE: Create via Assets > Create > Environment Preset
// ============================================

using UnityEngine;

[CreateAssetMenu(fileName = "New Environment Preset", menuName = "Environment Preset")]
public class EnvironmentPreset : ScriptableObject
{
    // ===== Skybox (Procedural) =====
    [Header("Skybox - Sun")]
    [Range(0f, 1f)]
    public float sunSize = 0.09f;

    [Range(1f, 10f)]
    public float sunSizeConvergence = 10f;

    [Header("Skybox - Atmosphere")]
    [Range(0f, 5f)]
    public float atmosphereThickness = 3f;

    public Color skyTint = new Color(0.5f, 0.5f, 0.5f, 1f);
    public Color groundColor = new Color(0.369f, 0.349f, 0.341f, 1f);

    [Header("Skybox - Exposure")]
    [Range(0f, 8f)]
    public float skyboxExposure = 2.6f;

    // ===== Fog =====
    [Header("Fog")]
    public bool fogEnabled = true;
    public FogMode fogMode = FogMode.ExponentialSquared;

    [Range(0f, 0.1f)]
    public float fogDensity = 0.01f;

    public Color fogColor = Color.gray;

    [Header("Fog - Linear Mode")]
    public float fogStart = 0f;
    public float fogEnd = 300f;

    // ===== Directional Light =====
    [Header("Directional Light")]
    [Range(0f, 8f)]
    public float lightIntensity = 1f;

    public Color lightColor = Color.white;

    // ===== Ambient =====
    [Header("Ambient")]
    public Color ambientColor = new Color(0.212f, 0.227f, 0.259f, 1f);

    [Range(0f, 8f)]
    public float ambientIntensity = 1f;
}
