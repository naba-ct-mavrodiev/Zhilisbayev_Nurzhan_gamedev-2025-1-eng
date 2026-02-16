// ============================================
// Shoot Action
// ============================================
// PURPOSE: Spawns a projectile and applies force toward a target
// USAGE: Attach to NPC, set projectile prefab and target transform
// ACTIONS:
//   - Shoot() - fire a single projectile at target
//   - ShootAtPosition(Vector3) - fire at specific world position
// ============================================

using UnityEngine;
using UnityEngine.Events;

public class ShootAction : MonoBehaviour
{
    // ===== Target =====
    [Header("Target")]
    [Tooltip("The transform to shoot at (usually the player)")]
    public Transform target;
    
    [Header("Shooter Identity")]
    [Tooltip("Tag of the shooter (Player or Enemy) to prevent self-damage")]
    public string shooterTag = "";

    [Tooltip("Where the projectile spawns from (gun barrel, hand, etc.)")]
    public Transform shootPoint;

    // ===== Projectile =====
    [Header("Projectile")]
    public GameObject projectilePrefab;

    [Tooltip("Force applied to projectile toward target")]
    public float projectileForce = 15f;

    [Tooltip("Impulse = instant burst, Force = continuous")]
    public ForceMode forceMode = ForceMode.Impulse;

    // ===== Settings =====
    [Header("Settings")]
    [Tooltip("Time between shots")]
    public float cooldown = 1f;

    [Tooltip("Aim at target's center height offset")]
    public float targetHeightOffset = 1f;

    [Tooltip("Random spread angle for inaccuracy")]
    [Range(0f, 30f)]
    public float spreadAngle = 0f;

    [Tooltip("Destroy projectile after this time (0 = never)")]
    public float projectileLifetime = 5f;
    
    public bool isShooting = false;

    // ===== Events =====
    [Header("Events")]
    public UnityEvent OnShoot;

    // ===== State =====
    private float nextShootTime = 0f;

    // ===== Initialization =====
    private void Awake()
    {
        if (shootPoint == null)
            shootPoint = transform;
    }

    public void Update()
    {
        if (isShooting) Shoot();
    }

    // ===== Actions =====
    [ContextMenu("Shoot")]
    public void Shoot()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + Vector3.up * targetHeightOffset;
        ShootAtPosition(targetPosition);
    }

    [ContextMenu("StartShooting")]
    public void StartShooting()
    {
        isShooting = true;
    }

    [ContextMenu("StopShooting")]
    public void StopShooting()
    {
        isShooting = false;
    }

    public void ShootAtPosition(Vector3 targetPosition)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile Prefab is NULL!");
            return;
        }
        if (Time.time < nextShootTime) return;

        // Calculate direction toward target
        Vector3 spawnPos = shootPoint.position;
        Vector3 direction = (targetPosition - spawnPos).normalized;

        // Apply spread if set
        if (spreadAngle > 0f)
        {
            direction = ApplySpread(direction, spreadAngle);
        }

        // Spawn projectile
        Quaternion rotation = Quaternion.LookRotation(direction);
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, rotation);
        Debug.Log("PROJECTILE SPAWNED at: " + spawnPos);

        // Set shooter tag
        Projectile proj = projectile.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.SetShooterTag(shooterTag);
            Debug.Log("Set shooter tag to: " + shooterTag);
        }

        // Apply force toward target
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(direction * projectileForce, forceMode);
            Debug.Log("Force applied: " + (direction * projectileForce));
        }
        else
        {
            Debug.LogError("NO RIGIDBODY ON PROJECTILE!");
        }

        // Auto-destroy projectile
        if (projectileLifetime > 0f)
        {
            Destroy(projectile, projectileLifetime);
        }

        // Set cooldown
        nextShootTime = Time.time + cooldown;

        // Fire event
        OnShoot?.Invoke();
    }

    // ===== Helpers =====
    public bool CanShoot()
    {
        return Time.time >= nextShootTime && projectilePrefab != null;
    }

    public bool HasTarget()
    {
        return target != null;
    }

    private Vector3 ApplySpread(Vector3 direction, float angle)
    {
        float spreadX = Random.Range(-angle, angle);
        float spreadY = Random.Range(-angle, angle);
        Quaternion spreadRotation = Quaternion.Euler(spreadX, spreadY, 0f);
        return spreadRotation * direction;
    }

    // ===== Gizmos =====
    private void OnDrawGizmosSelected()
    {
        Vector3 shootPos = shootPoint != null ? shootPoint.position : transform.position;

        // Shoot point
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(shootPos, 0.15f);

        // Direction to target
        if (target != null)
        {
            Vector3 targetPos = target.position + Vector3.up * targetHeightOffset;

            // Line to target
            Gizmos.color = CanShoot() ? Color.green : Color.gray;
            Gizmos.DrawLine(shootPos, targetPos);

            // Target indicator
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(targetPos, 0.25f);
        }
        else
        {
            // Forward direction when no target
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(shootPos, (shootPoint != null ? shootPoint.forward : transform.forward) * 3f);
        }

        // Spread cone visualization
        if (spreadAngle > 0f)
        {
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
            Vector3 forward = shootPoint != null ? shootPoint.forward : transform.forward;
            DrawSpreadCone(shootPos, forward, spreadAngle, 2f);
        }
    }

    private void DrawSpreadCone(Vector3 origin, Vector3 direction, float angle, float length)
    {
        Vector3 right = Quaternion.Euler(0, angle, 0) * direction;
        Vector3 left = Quaternion.Euler(0, -angle, 0) * direction;
        Vector3 up = Quaternion.Euler(-angle, 0, 0) * direction;
        Vector3 down = Quaternion.Euler(angle, 0, 0) * direction;

        Gizmos.DrawRay(origin, right * length);
        Gizmos.DrawRay(origin, left * length);
        Gizmos.DrawRay(origin, up * length);
        Gizmos.DrawRay(origin, down * length);
    }
}

// ============================================
// IMPLEMENTATION STEPS
// ============================================
// 1. Attach to NPC GameObject
// 2. Set target to Player transform
// 3. Set shootPoint to gun barrel or hand bone
// 4. Assign projectile prefab (must have Rigidbody)
// 5. Adjust force, cooldown, and spread as needed
// 6. Call Shoot() from triggers or other scripts
// ============================================