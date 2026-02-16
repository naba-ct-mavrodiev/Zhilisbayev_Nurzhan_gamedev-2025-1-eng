using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform enemyTarget; // The enemy to follow
    [SerializeField] private Health enemyHealth;
    [SerializeField] private Image healthBarFill;
    
    [Header("Settings")]
    [SerializeField] private Vector3 worldOffset = new Vector3(0, 2.5f, 0);
    [SerializeField] private bool hideWhenFull = true;
    [SerializeField] private bool alwaysFaceCamera = true;
    
    [Header("Colors")]
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color lowHealthColor = Color.red;
    
    private Camera mainCamera;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private bool isInitialized = false;
    
    void Awake()
    {
        canvas = GetComponent<Canvas>();
        
        // Ensure World Space mode
        if (canvas != null && canvas.renderMode != RenderMode.WorldSpace)
        {
            canvas.renderMode = RenderMode.WorldSpace;
            Debug.LogWarning("EnemyHealthBar: Changed Canvas to World Space mode");
        }
        
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }
    
    void Start()
    {
        StartCoroutine(InitializeHealthBar());
    }
    
    IEnumerator InitializeHealthBar()
    {
        yield return new WaitForEndOfFrame();
        
        mainCamera = Camera.main;
        
        // Auto-find references if not set
        if (enemyTarget == null && transform.parent != null)
        {
            enemyTarget = transform.parent;
        }
        
        if (enemyHealth == null && enemyTarget != null)
        {
            enemyHealth = enemyTarget.GetComponent<Health>();
        }
        
        // Validate
        if (enemyTarget == null)
        {
            Debug.LogError("EnemyHealthBar: No enemy target assigned!");
            enabled = false;
            yield break;
        }
        
        if (enemyHealth == null)
        {
            Debug.LogError("EnemyHealthBar: No Health component found!");
            enabled = false;
            yield break;
        }
        
        if (healthBarFill == null)
        {
            Debug.LogError("EnemyHealthBar: No health bar fill image assigned!");
            enabled = false;
            yield break;
        }
        
        // Subscribe
        enemyHealth.OnHealthChanged.AddListener(UpdateHealthBar);
        UpdateHealthBar(enemyHealth.GetHealthPercentage());
        
        isInitialized = true;
        
        Debug.Log($"EnemyHealthBar initialized for {enemyTarget.name}");
    }
    
    void LateUpdate()
    {
        if (!isInitialized || enemyTarget == null) return;
        
        // Follow enemy position (this only moves the health bar, not the enemy!)
        transform.position = enemyTarget.position + worldOffset;
        
        // Always face camera
        if (alwaysFaceCamera && mainCamera != null)
        {
            transform.rotation = mainCamera.transform.rotation;
        }
    }
    
    void UpdateHealthBar(float healthPercentage)
    {
        if (healthBarFill == null) return;
        
        // Update fill amount
        healthBarFill.fillAmount = healthPercentage;
        
        // Update color
        healthBarFill.color = Color.Lerp(lowHealthColor, fullHealthColor, healthPercentage);
        
        // Show/hide when full
        if (hideWhenFull && canvasGroup != null)
        {
            canvasGroup.alpha = healthPercentage >= 1f ? 0f : 1f;
        }
    }
    
    void OnDestroy()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnHealthChanged.RemoveListener(UpdateHealthBar);
        }
    }
}
// ```
//
// ---
//
// ## **Step 4: Attach to Enemy**
//
// ### **Option A: As Child (Easier):**
//
// 1. **Drag EnemyHealthBar** onto Enemy in Hierarchy (makes it a child)
// 2. **Select EnemyHealthBar**
// 3. **In EnemyHealthBar script:**
// ```
//    Enemy Target: [Leave Empty] ← Auto-finds parent
//    Enemy Health: [Leave Empty] ← Auto-finds
//    Health Bar Fill: [Drag Fill image]
//    World Offset: (0, 2.5, 0) ← Height above enemy
// ```
//
// ### **Option B: Separate (Cleaner):**
//
// 1. **Keep EnemyHealthBar separate** in Hierarchy
// 2. **In EnemyHealthBar script:**
// ```
//    Enemy Target: [Drag Enemy GameObject]
//    Enemy Health: [Drag Enemy GameObject]
//    Health Bar Fill: [Drag Fill image]
// ```
//
// ---
//
// ## **Step 5: Make it a Prefab**
//
// 1. **Drag EnemyHealthBar** to Project folder
// 2. **Now drag this prefab** onto each enemy
//
// OR
//
// 3. **Include in Enemy prefab** if you want
//
// ---
//
// ## **Why You Can't See It - Common Issues:**
//
// ### **Issue 1: Canvas Scale Too Small/Big**
// ```
// Canvas Rect Transform:
// └─ Scale: (0.01, 0.01, 0.01) ← Try adjusting between 0.005 - 0.02
// ```
//
// ### **Issue 2: Canvas Size Too Small**
// ```
// Rect Transform:
// ├─ Width: 100 ← Try making bigger (200)
// └─ Height: 20 ← Try making bigger (40)
// ```
//
// ### **Issue 3: Offset Too High/Low**
// ```
// World Offset: (0, 2.5, 0) ← Try different values:
//   - Too high: (0, 5, 0)
//   - Too low: (0, 0.5, 0)
//   - Try: (0, 2, 0)
// ```
//
// ### **Issue 4: Behind Enemy**
//
// The health bar might be behind the enemy model. Try:
// ```
// World Offset: (0, 2.5, 0.5) ← Add Z offset to bring forward
// ```
//
// ### **Issue 5: Canvas Not World Space**
// ```
// Canvas:
// └─ Render Mode: MUST BE World Space!