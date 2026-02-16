using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private bool isPlayer = false;
    
    [Header("Player Death Settings")]
    [SerializeField] private float restartDelay = 3f;
    [SerializeField] private GameObject gameOverPanel;
    
    private float currentHealth;
    
    public UnityEvent<float> OnHealthChanged;
    public UnityEvent OnDeath;
    
    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"[{gameObject.name}] Health initialized: {currentHealth}/{maxHealth}");
        
        // Hide game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }
    
    public void TakeDamage(float damage)
    {
        float previousHealth = currentHealth;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        Debug.Log($"[{gameObject.name}] TOOK DAMAGE: {damage}");
        Debug.Log($"[{gameObject.name}] Health: {previousHealth} → {currentHealth} ({GetHealthPercentage() * 100:F1}%)");
        
        OnHealthChanged?.Invoke(currentHealth / maxHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Heal(float amount)
    {
        float previousHealth = currentHealth;
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        Debug.Log($"[{gameObject.name}] HEALED: {amount}");
        Debug.Log($"[{gameObject.name}] Health: {previousHealth} → {currentHealth}");
        
        OnHealthChanged?.Invoke(currentHealth / maxHealth);
    }
    
    void Die()
    {
        Debug.Log($"[{gameObject.name}] ☠️ DIED! Health reached 0");
        OnDeath?.Invoke();
        
        if (isPlayer)
        {
            // PLAYER DIED - END GAME
            HandlePlayerDeath();
        }
        else
        {
            // Enemy died
            Debug.Log($"Enemy {gameObject.name} destroyed");
            Destroy(gameObject, 0.5f);
        }
    }
    
    void HandlePlayerDeath()
    {
        Debug.Log("GAME OVER - Player died!");
        
        // Show game over UI
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        // Optional: Stop time
        // Time.timeScale = 0f;
        
        // Restart game after delay
        Invoke(nameof(RestartGame), restartDelay);
    }
    
    void RestartGame()
    {
        // Restore time if stopped
        Time.timeScale = 1f;
        
        // Reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
    public float GetHealthPercentage() => currentHealth / maxHealth;
}