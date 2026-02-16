using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform player;
    [SerializeField] private EnemySight sight;
    
    [Header("Wandering")]
    [SerializeField] private float wanderRadius = 10f;
    [SerializeField] private float wanderTimer = 5f;
    [SerializeField] private float idleTime = 2f;
    
    [Header("Chasing")]
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float losePlayerTime = 5f;
    
    [Header("Attack")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float damage = 10f;
    
    [Header("Events")]
    public UnityEvent OnPlayerDetected;      // When first sees player
    public UnityEvent OnStartChasing;        // When starts chasing
    public UnityEvent OnStopChasing;         // When stops chasing
    public UnityEvent OnStartAttacking;      // When enters attack range
    public UnityEvent OnAttack;              // Each attack
    public UnityEvent OnLosePlayer;          // When loses sight of player
    public UnityEvent OnStartWandering;      // When starts wandering
    public UnityEvent OnIdle;                // When goes idle
    
    // State
    private enum State { Idle, Wandering, Chasing, Attacking }
    private State currentState = State.Idle;
    private State previousState;
    
    private float stateTimer;
    private float lastAttackTime;
    private float lastSeenPlayerTime;
    private Vector3 lastKnownPlayerPosition;
    private bool hasDetectedPlayer = false;  // Track if player was detected this session
    
    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player")?.transform;
    
        // Auto-find Sight if not assigned
        if (sight == null) sight = GetComponentInChildren<EnemySight>();
    
        stateTimer = wanderTimer;
        ChangeState(State.Idle);
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (sight == null) sight = GetComponentInChildren<EnemySight>();
        
        stateTimer = wanderTimer;
        ChangeState(State.Idle);
    }
    
    void Update()
    {
        // Check if player is visible
        bool canSeePlayer = sight != null && sight.CanSeePlayer();
        
        if (canSeePlayer)
        {
            // First time detecting player
            if (!hasDetectedPlayer)
            {
                hasDetectedPlayer = true;
                OnPlayerDetected?.Invoke();
            }
            
            lastSeenPlayerTime = Time.time;
            lastKnownPlayerPosition = player.position;
        }
        
        // State machine
        switch (currentState)
        {
            case State.Idle:
                HandleIdle();
                break;
            case State.Wandering:
                HandleWandering(canSeePlayer);
                break;
            case State.Chasing:
                HandleChasing(canSeePlayer);
                break;
            case State.Attacking:
                HandleAttacking(canSeePlayer);
                break;
        }
        
        // Update animator
        UpdateAnimator();
    }
    
    void HandleIdle()
    {
        stateTimer -= Time.deltaTime;
        
        if (stateTimer <= 0)
        {
            ChangeState(State.Wandering);
        }
    }
    
    void HandleWandering(bool canSeePlayer)
    {
        // If see player, start chasing
        if (canSeePlayer)
        {
            ChangeState(State.Chasing);
            return;
        }
        
        // Check if reached destination
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            stateTimer -= Time.deltaTime;
            
            if (stateTimer <= 0)
            {
                Vector3 newPos = GetRandomPoint(transform.position, wanderRadius);
                agent.SetDestination(newPos);
                stateTimer = wanderTimer;
            }
        }
    }
    
    void HandleChasing(bool canSeePlayer)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // If close enough, start attacking
        if (distanceToPlayer <= attackRange)
        {
            ChangeState(State.Attacking);
            return;
        }
        
        // If can see player, chase them
        if (canSeePlayer)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            // Go to last known position
            agent.SetDestination(lastKnownPlayerPosition);
            
            // If lost player for too long, return to wandering
            if (Time.time - lastSeenPlayerTime > losePlayerTime)
            {
                OnLosePlayer?.Invoke();
                ChangeState(State.Wandering);
            }
        }
    }
    
    void HandleAttacking(bool canSeePlayer)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // Stop moving
        agent.SetDestination(transform.position);
        
        // Look at player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        
        // If player moved too far, chase again
        if (distanceToPlayer > attackRange * 1.5f)
        {
            ChangeState(State.Chasing);
            return;
        }
        
        // Attack
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }
    
    void Attack()
    {
        // Trigger event
        OnAttack?.Invoke();
        
        // Play attack animation
        if (animator != null)
        {
            animator.SetTrigger("attack");
        }
        
        // Deal damage if player is in range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"Enemy attacked player for {damage} damage!");
            }
        }
    }
    
    void ChangeState(State newState)
    {
        // Don't trigger events if staying in same state
        if (currentState == newState) return;
        
        previousState = currentState;
        currentState = newState;
        
        // Trigger state exit events
        switch (previousState)
        {
            case State.Chasing:
                OnStopChasing?.Invoke();
                break;
        }
        
        // Trigger state enter events and setup
        switch (newState)
        {
            case State.Idle:
                agent.isStopped = true;
                stateTimer = idleTime;
                OnIdle?.Invoke();
                break;
                
            case State.Wandering:
                agent.isStopped = false;
                agent.speed = chaseSpeed * 0.5f;
                Vector3 newPos = GetRandomPoint(transform.position, wanderRadius);
                agent.SetDestination(newPos);
                stateTimer = wanderTimer;
                OnStartWandering?.Invoke();
                break;
                
            case State.Chasing:
                agent.isStopped = false;
                agent.speed = chaseSpeed;
                OnStartChasing?.Invoke();
                break;
                
            case State.Attacking:
                agent.isStopped = true;
                OnStartAttacking?.Invoke();
                break;
        }
    }
    
    void UpdateAnimator()
    {
        if (animator == null) return;
        
        // Set speed parameter for blend tree
        float speed = agent.velocity.magnitude;
        animator.SetFloat("speed", speed);
        
        // Set state bools if you need them
        animator.SetBool("isChasing", currentState == State.Chasing);
        animator.SetBool("isAttacking", currentState == State.Attacking);
    }
    
    Vector3 GetRandomPoint(Vector3 center, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += center;
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        
        return center;
    }
    
    // Gizmos for debugging
    void OnDrawGizmosSelected()
    {
        // Wander radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
        
        // Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        // Last known player position
        if (lastKnownPlayerPosition != Vector3.zero)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(lastKnownPlayerPosition, 0.5f);
        }
    }
}
// ```
//
// ## How to Use the Events in Inspector:
//
// ### 1. **Setup Animator Triggers**
// In the Inspector, expand the "Events" section of the EnemyAI component:
//
// **For OnStartChasing:**
// - Click the `+` button
// - Drag your enemy GameObject into the object field
// - Select `Animator → SetTrigger(string)`
// - Type "run" (or whatever your run trigger is called)
//
// **For OnStartAttacking:**
// - Same steps but use your attack trigger name
//
// ### 2. **Example Setup:**
// ```
// OnPlayerDetected:
//   - Animator.SetTrigger("alert")    // Play alert animation
//   - AudioSource.Play()              // Play alert sound
//
// OnStartChasing:
//   - Animator.SetTrigger("run")      // Start running animation
//   - ParticleSystem.Play()           // Angry effect
//
// OnStartAttacking:
//   - Animator.SetBool("isAttacking") // Set attacking bool
//
// OnAttack:
//   - AudioSource.Play()              // Attack sound
//   - ParticleSystem.Play()           // Attack effect
//
// OnLosePlayer:
//   - Animator.SetTrigger("confused") // Confused animation
// ```
//
// ### 3. **Visual Guide in Inspector:**
//
// When you select your enemy, you'll see:
// ```
// Events ▼
//   ├─ On Player Detected (0)
//   ├─ On Start Chasing (0)
//   ├─ On Stop Chasing (0)
//   ├─ On Start Attacking (0)
//   ├─ On Attack (0)
//   ├─ On Lose Player (0)
//   ├─ On Start Wandering (0)
//   └─ On Idle (0)