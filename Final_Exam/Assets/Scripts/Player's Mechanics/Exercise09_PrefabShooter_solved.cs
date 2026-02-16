using UnityEngine;
using UnityEngine.Events;

// ============================================
// EXERCISE 09: Prefab Shooter
// DIFFICULTY: Medium
// CONCEPTS: Instantiate, Rigidbody, AddForce, Transform.forward
//
// INSTRUCTIONS:
// 1. Uncomment the lines marked "UNCOMMENT TO FIX"
// 2. Find and fix 5 bugs in this script
// 3. Create a prefab with a Rigidbody component
// 4. Assign the prefab and spawnPoint in Inspector
// 5. Test using the ContextMenu "Shoot" option (in Play mode)
//
// SETUP:
// - Create a Sphere, add Rigidbody, make it a Prefab
// - Create an empty GameObject as spawnPoint (position it in front of player)
// - Assign both to this script
// ============================================

public class Exercise09_PrefabShooter_solved : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The projectile prefab to spawn (must have Rigidbody)")]
    public GameObject projectilePrefab;

    [Tooltip("Where projectiles spawn from")]
    public Transform spawnPoint;

    [Tooltip("How much force to apply")]
    public float shootForce = 500f;

    [Tooltip("How long before projectile is destroyed")]
    public float projectileLifetime = 5f;

    [Header("Events")]
    public UnityEvent ShotFiredEvent;

    [ContextMenu("Shoot")]
    public void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);

        if (projectile != null)
        {
            Rigidbody rb;
            
            rb = projectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 shootDirection = spawnPoint.up;
                
                shootDirection = Vector3.forward;

                rb.AddForce(shootDirection * shootForce);
                Debug.Log("Projectile fired with force: " + shootForce);
            }
            else
            {
                Debug.LogWarning("Projectile has no Rigidbody!");
            }
            
            Destroy(projectile, projectileLifetime);
        }

        ShotFiredEvent?.Invoke();
    }

    // Visualization helper - shows spawn point and direction in Scene view
    private void OnDrawGizmosSelected()
    {
        if (spawnPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(spawnPoint.position, 0.1f);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(spawnPoint.position, spawnPoint.forward * 2f);
        }
    }
}
