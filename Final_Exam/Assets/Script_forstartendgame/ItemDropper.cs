using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [Header("Drop Settings")]
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private int minDropAmount = 1;
    [SerializeField] private int maxDropAmount = 3;
    
    [Header("Drop Physics")]
    [SerializeField] private float dropForce = 5f;
    [SerializeField] private float dropRadius = 1f;
    [SerializeField] private bool randomizeRotation = true;
    
    [Header("Drop Chance")]
    [SerializeField] private bool alwaysDrop = true;
    [SerializeField] [Range(0f, 1f)] private float dropChance = 1f;
    
    private Health health;
    
    void Start()
    {
        health = GetComponent<Health>();
        
        if (health != null)
        {
            // Subscribe to death event
            health.OnDeath.AddListener(DropItems);
        }
        else
        {
            Debug.LogWarning($"ItemDropper on {gameObject.name}: No Health component found!");
        }
    }
    
    void DropItems()
    {
        if (itemPrefab == null)
        {
            Debug.LogWarning($"ItemDropper on {gameObject.name}: No item prefab assigned!");
            return;
        }
        
        // Check drop chance
        if (!alwaysDrop && Random.value > dropChance)
        {
            Debug.Log($"{gameObject.name} didn't drop anything (chance)");
            return;
        }
        
        // Determine how many items to drop
        int dropCount = Random.Range(minDropAmount, maxDropAmount + 1);
        
        for (int i = 0; i < dropCount; i++)
        {
            SpawnItem();
        }
        
        Debug.Log($"{gameObject.name} dropped {dropCount} items");
    }
    
    void SpawnItem()
    {
        // Random position around enemy
        Vector3 randomOffset = Random.insideUnitSphere * dropRadius;
        randomOffset.y = 0.5f; // Spawn slightly above ground
        Vector3 spawnPos = transform.position + randomOffset;
        
        // Random rotation
        Quaternion rotation = randomizeRotation 
            ? Quaternion.Euler(0, Random.Range(0f, 360f), 0) 
            : Quaternion.identity;
        
        // Spawn the item
        GameObject droppedItem = Instantiate(itemPrefab, spawnPos, rotation);
        
        // Add some physics if item has Rigidbody
        Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 randomDirection = Random.insideUnitSphere;
            randomDirection.y = Mathf.Abs(randomDirection.y); // Always upward
            rb.AddForce(randomDirection * dropForce, ForceMode.Impulse);
        }
    }
    
    void OnDestroy()
    {
        if (health != null)
        {
            health.OnDeath.RemoveListener(DropItems);
        }
    }
}
// ```
//
// ---
//
// ## **Setup Instructions:**
//
// ### **Step 1: Create Item Prefab**
//
// 1. **Create an item GameObject** (e.g., Cube, Sphere, or your custom model)
// 2. **Rename it** (e.g., "HealthPotion", "Coin", "Ammo")
// 3. **Add components:**
// ```
//    Item GameObject:
//    ├─ Mesh/Model (visual)
//    ├─ Collider (Box/Sphere - Is Trigger: ✓)
//    ├─ Rigidbody (optional, for physics)
//    └─ Item pickup script (optional, see below)
// ```
// 4. **Drag to Project folder** to make it a prefab
// 5. **Delete from scene**
//
// ### **Step 2: Add to Enemy**
//
// 1. **Select Enemy prefab**
// 2. **Add Component → Item Dropper**
// 3. **Settings:**
// ```
//    ItemDropper:
//    ├─ Item Prefab: [Drag your item prefab]
//    ├─ Min Drop Amount: 1
//    ├─ Max Drop Amount: 3
//    ├─ Drop Force: 5
//    ├─ Drop Radius: 1
//    ├─ Randomize Rotation: ✓
//    ├─ Always Drop: ✓
//    └─ Drop Chance: 1 (100%)