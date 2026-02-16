using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    // My valuables for the health system

    public int health = 100;
    
    public int maxHealth = 100;
    
    public int currentHealth;

    public bool isDead;
    
    //Events

    public UnityEvent onDamage;
    public UnityEvent onHeal;
    public UnityEvent onDeath;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            health = 0;
        }

        if (isDead)
        {
            onDeath.Invoke();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        onDamage.Invoke();
    }

    public void Heal(int heal)
    {
        health += heal;
        onHeal.Invoke();
    }

    public bool IsDead()
    {
        return health <= 0;
    }
    
}
