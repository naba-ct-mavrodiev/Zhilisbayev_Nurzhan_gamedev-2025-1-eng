// ============================================
// Camera Shake Action (Cinemachine 3)
// ============================================
// PURPOSE: Triggers camera shake using Cinemachine Impulse
// USAGE: Attach to any GameObject with CinemachineImpulseSource
// ACTIONS:
//   - Shake() - trigger shake
//   - LightShake() / HeavyShake() - presets
// ============================================

using UnityEngine;
using Unity.Cinemachine;

public class CameraShakeAction : MonoBehaviour
{
    // ===== Target =====
    [Header("Target")]
    [Tooltip("The impulse source. If null, will try to get from this GameObject.")]
    public CinemachineImpulseSource impulseSource;

    // ===== Settings =====
    [Header("Settings")]
    public float force = 1f;
    public Vector3 velocity = Vector3.down;

    // ===== Lifecycle =====
    private void Awake()
    {
        if (impulseSource == null)
        {
            impulseSource = GetComponent<CinemachineImpulseSource>();
        }
    }

    // ===== Actions =====
    [ContextMenu("Shake")]
    public void Shake()
    {
        if (impulseSource == null)
        {
            Debug.LogWarning("CameraShakeAction: No CinemachineImpulseSource found!", this);
            return;
        }

        impulseSource.GenerateImpulse(velocity * force);
    }

    public void Shake(float customForce)
    {
        if (impulseSource == null) return;
        impulseSource.GenerateImpulse(velocity * customForce);
    }

    [ContextMenu("Light Shake")]
    public void LightShake()
    {
        if (impulseSource == null) return;
        impulseSource.GenerateImpulse(velocity * 0.3f);
    }

    [ContextMenu("Heavy Shake")]
    public void HeavyShake()
    {
        if (impulseSource == null) return;
        impulseSource.GenerateImpulse(velocity * 2f);
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Add CinemachineImpulseSource component to this GameObject
// 2. Configure impulse settings on the source (duration, shape, etc.)
// 3. Add CinemachineImpulseListener to your Cinemachine Camera
// 4. Attach this action and set force/velocity
// 5. Call Shake() from triggers
// ============================================
