// ============================================
// Environment Controller
// ============================================
// PURPOSE: Controls all environment settings (skybox, fog, light, ambient)
// USAGE: Attach to any GameObject, assign directional light and presets
// ACTIONS:
//   - ApplyPreset(preset) - snap to preset instantly
//   - TransitionTo(preset, duration) - smoothly transition to preset
//   - TransitionToPreset() - transition to assigned preset with set duration
// ============================================

using UnityEngine;
using DG.Tweening;

public class EnvironmentController : MonoBehaviour
{
    // ===== Target =====
    [Header("Target")]
    public Light directionalLight;

    // ===== Preset =====
    [Header("Preset")]
    public EnvironmentPreset preset;
    public float transitionDuration = 2f;

    // ===== Current Values =====
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

    [Header("Fog")]
    public bool fogEnabled = true;

    [Range(0f, 0.1f)]
    public float fogDensity = 0.01f;

    public Color fogColor = Color.gray;

    [Header("Directional Light")]
    [Range(0f, 8f)]
    public float lightIntensity = 1f;

    public Color lightColor = Color.white;

    [Header("Ambient")]
    public Color ambientColor = new Color(0.212f, 0.227f, 0.259f, 1f);

    // ===== Cached =====
    private Material skyboxMaterial;
    private bool isInitialized = false;

    private static readonly int SunSizeID = Shader.PropertyToID("_SunSize");
    private static readonly int SunSizeConvergenceID = Shader.PropertyToID("_SunSizeConvergence");
    private static readonly int AtmosphereThicknessID = Shader.PropertyToID("_AtmosphereThickness");
    private static readonly int SkyTintID = Shader.PropertyToID("_SkyTint");
    private static readonly int GroundColorID = Shader.PropertyToID("_GroundColor");
    private static readonly int ExposureID = Shader.PropertyToID("_Exposure");

    // ===== Lifecycle =====
    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        ApplyEffect();
    }

    // ===== Actions =====
    [ContextMenu("Apply Preset (Instant)")]
    public void ApplyPresetInstant()
    {
        if (preset != null)
            ApplyPreset(preset);
    }

    [ContextMenu("Transition To Preset")]
    public void TransitionToPreset()
    {
        if (preset != null)
            TransitionTo(preset, transitionDuration);
    }

    public void ApplyPreset(EnvironmentPreset p)
    {
        if (p == null) return;

        // Skybox
        sunSize = p.sunSize;
        sunSizeConvergence = p.sunSizeConvergence;
        atmosphereThickness = p.atmosphereThickness;
        skyTint = p.skyTint;
        groundColor = p.groundColor;
        skyboxExposure = p.skyboxExposure;

        // Fog
        fogEnabled = p.fogEnabled;
        fogDensity = p.fogDensity;
        fogColor = p.fogColor;
        RenderSettings.fogMode = p.fogMode;
        RenderSettings.fogStartDistance = p.fogStart;
        RenderSettings.fogEndDistance = p.fogEnd;

        // Light
        lightIntensity = p.lightIntensity;
        lightColor = p.lightColor;

        // Ambient
        ambientColor = p.ambientColor;
        RenderSettings.ambientIntensity = p.ambientIntensity;

        ApplyEffect();
    }

    public void TransitionTo(EnvironmentPreset p, float duration)
    {
        if (p == null) return;

        // Skybox floats
        DOTween.To(() => sunSize, x => sunSize = x, p.sunSize, duration);
        DOTween.To(() => sunSizeConvergence, x => sunSizeConvergence = x, p.sunSizeConvergence, duration);
        DOTween.To(() => atmosphereThickness, x => atmosphereThickness = x, p.atmosphereThickness, duration);
        DOTween.To(() => skyboxExposure, x => skyboxExposure = x, p.skyboxExposure, duration);

        // Skybox colors
        DOTween.To(() => skyTint, x => skyTint = x, p.skyTint, duration);
        DOTween.To(() => groundColor, x => groundColor = x, p.groundColor, duration);

        // Fog
        fogEnabled = p.fogEnabled;
        RenderSettings.fogMode = p.fogMode;
        RenderSettings.fogStartDistance = p.fogStart;
        RenderSettings.fogEndDistance = p.fogEnd;
        DOTween.To(() => fogDensity, x => fogDensity = x, p.fogDensity, duration);
        DOTween.To(() => fogColor, x => fogColor = x, p.fogColor, duration);

        // Light
        DOTween.To(() => lightIntensity, x => lightIntensity = x, p.lightIntensity, duration);
        DOTween.To(() => lightColor, x => lightColor = x, p.lightColor, duration);

        // Ambient
        DOTween.To(() => ambientColor, x => ambientColor = x, p.ambientColor, duration);
        DOTween.To(() => RenderSettings.ambientIntensity, x => RenderSettings.ambientIntensity = x, p.ambientIntensity, duration);
    }

    // ===== Internal =====
    private void Initialize()
    {
        skyboxMaterial = RenderSettings.skybox;

        if (directionalLight == null)
            directionalLight = FindAnyObjectByType<Light>();

        isInitialized = skyboxMaterial != null;

        if (preset != null)
            ApplyPreset(preset);
        else
            ApplyEffect();
    }

    private void ApplyEffect()
    {
        // Skybox
        if (isInitialized && skyboxMaterial != null)
        {
            skyboxMaterial.SetFloat(SunSizeID, sunSize);
            skyboxMaterial.SetFloat(SunSizeConvergenceID, sunSizeConvergence);
            skyboxMaterial.SetFloat(AtmosphereThicknessID, atmosphereThickness);
            skyboxMaterial.SetColor(SkyTintID, skyTint);
            skyboxMaterial.SetColor(GroundColorID, groundColor);
            skyboxMaterial.SetFloat(ExposureID, skyboxExposure);
        }

        // Fog
        RenderSettings.fog = fogEnabled;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.fogColor = fogColor;

        // Light
        if (directionalLight != null)
        {
            directionalLight.intensity = lightIntensity;
            directionalLight.color = lightColor;
        }

        // Ambient
        RenderSettings.ambientLight = ambientColor;
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to any GameObject in the scene
// 2. Assign Directional Light reference
// 3. Create presets: Assets > Create > Environment Preset
// 4. Configure presets for different times/weather (Day, Night, Sunset, Foggy)
// 5. Assign preset and call TransitionToPreset() or ApplyPresetInstant()
// 6. Can also adjust values directly in Inspector for animation
// ============================================
