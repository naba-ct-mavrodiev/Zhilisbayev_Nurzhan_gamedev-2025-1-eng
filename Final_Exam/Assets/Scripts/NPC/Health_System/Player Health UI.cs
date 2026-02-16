using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this for TextMeshPro support

public class PlayerHealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TextMeshProUGUI healthText; // Changed to TextMeshPro
    
    [Header("Colors")]
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color lowHealthColor = Color.red;
    [SerializeField] private float lowHealthThreshold = 0.3f;
    
    [Header("Animation")]
    [SerializeField] private bool animateSmooth = true;
    [SerializeField] private float smoothSpeed = 5f;
    
    private float targetFillAmount = 1f; // Start at full
    private bool isInitialized = false;
    
    void Start()
    {
        if (playerHealth == null)
        {
            playerHealth = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Health>();
        }
    
        if (playerHealth != null)
        {
            // Wait one frame for Health to initialize
            StartCoroutine(InitializeHealthBar());
        }
        else
        {
            Debug.LogError("PlayerHealthUI: No Health component found!");
        }
    }

    private System.Collections.IEnumerator InitializeHealthBar()
    {
        // Wait for end of frame to ensure Health.Start() has run
        yield return new WaitForEndOfFrame();
    
        // Now initialize
        float initialHealth = playerHealth.GetHealthPercentage();
        targetFillAmount = initialHealth;
    
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = initialHealth;
            healthBarFill.color = fullHealthColor;
        }
    
        // Subscribe to changes
        playerHealth.OnHealthChanged.AddListener(UpdateHealthBar);
    
        // Update display
        UpdateHealthBar(initialHealth);
    
        isInitialized = true;
    
        Debug.Log($"Health initialized: {playerHealth.GetCurrentHealth()} / {playerHealth.GetMaxHealth()}");
    }
    void Update()
    {
        if (!isInitialized) return;
        
        if (animateSmooth && healthBarFill != null)
        {
            // Smooth animation
            healthBarFill.fillAmount = Mathf.Lerp(healthBarFill.fillAmount, targetFillAmount, Time.deltaTime * smoothSpeed);
        }
    }
    
    void UpdateHealthBar(float healthPercentage)
    {
        targetFillAmount = healthPercentage;
        
        if (!animateSmooth && healthBarFill != null)
        {
            healthBarFill.fillAmount = healthPercentage;
        }
        
        // Update color based on health
        if (healthBarFill != null)
        {
            if (healthPercentage <= lowHealthThreshold)
            {
                // Low health - red
                healthBarFill.color = lowHealthColor;
            }
            else
            {
                // Interpolate between low and full health color
                float t = (healthPercentage - lowHealthThreshold) / (1f - lowHealthThreshold);
                healthBarFill.color = Color.Lerp(lowHealthColor, fullHealthColor, t);
            }
        }
        
        // Update text (optional)
        if (healthText != null && playerHealth != null)
        {
            healthText.text = $"{playerHealth.GetCurrentHealth():F0} / {playerHealth.GetMaxHealth():F0}";
        }
    }
    
    void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged.RemoveListener(UpdateHealthBar);
        }
    }
}