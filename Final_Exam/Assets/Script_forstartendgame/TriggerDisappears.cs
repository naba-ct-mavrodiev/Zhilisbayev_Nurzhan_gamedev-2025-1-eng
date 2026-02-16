using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float delay = 0f;
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private AudioClip destroySound;
    
    // Уничтожить этот объект
    public void DestroyThis()
    {
        Debug.Log($"Destroying {gameObject.name}");
        
        SpawnEffect();
        PlaySound();
        
        Destroy(gameObject, delay);
    }
    
    // Скрыть этот объект
    public void HideThis()
    {
        Debug.Log($"Hiding {gameObject.name}");
        
        SpawnEffect();
        PlaySound();
        
        gameObject.SetActive(false);
    }
    
    // Уничтожить с задержкой
    public void DestroyWithDelay(float seconds)
    {
        SpawnEffect();
        PlaySound();
        
        Destroy(gameObject, seconds);
    }
    
    void SpawnEffect()
    {
        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
        }
    }
    
    void PlaySound()
    {
        if (destroySound != null)
        {
            AudioSource.PlayClipAtPoint(destroySound, transform.position);
        }
    }
}