using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float damage = 20f;
    [SerializeField] private string shooterTag = "";
    
    [Header("Effects")]
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private float effectLifetime = 2f;
    
    [Header("Lifetime")]
    [SerializeField] private float lifetime = 5f;
    
    private bool hasHit = false; // ADD THIS - prevents multiple hits
    
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        // CRITICAL: Check if already hit something
        if (hasHit) return;
        hasHit = true; // Mark as hit immediately
        
        Debug.Log($"=== PROJECTILE HIT: {collision.gameObject.name} ===");
        
        // Don't hit the shooter
        if (!string.IsNullOrEmpty(shooterTag) && collision.gameObject.CompareTag(shooterTag))
        {
            Debug.Log("Ignoring - same tag as shooter");
            hasHit = false; // Reset so it can try to hit other things
            return;
        }
        
        // Detach particles
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particles)
        {
            ps.transform.SetParent(null);
            var emission = ps.emission;
            emission.enabled = false;
            float maxLifetime = ps.main.startLifetime.constantMax;
            Destroy(ps.gameObject, maxLifetime + 0.5f);
        }
        
        // Spawn hit effect
        if (hitEffect != null)
        {
            ContactPoint contact = collision.contacts[0];
            ParticleSystem effect = Instantiate(hitEffect, contact.point, Quaternion.LookRotation(contact.normal));
            Destroy(effect.gameObject, effectLifetime);
        }
        
        // Deal damage ONCE
        Health targetHealth = collision.gameObject.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage);
            Debug.Log($"✓ Dealt {damage} damage to {collision.gameObject.name}");
        }
        else
        {
            Debug.Log($"✗ No Health component on {collision.gameObject.name}");
        }
        
        // Hide mesh
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        if (mesh != null) mesh.enabled = false;
        
        // Disable collider
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        
        // Disable rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
        
        // Destroy projectile
        Destroy(gameObject, 0.2f);
    }
    
    public void SetShooterTag(string tag)
    {
        shooterTag = tag;
    }
}