using UnityEngine;

public class EnemySight : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float fieldOfView = 110f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstructionLayer;
    
    [Header("References")]
    [SerializeField] private Transform player;
    
    private bool playerInSight = false;
    
    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }
    
    void Update()
    {
        CheckForPlayer();
    }
    
    void CheckForPlayer()
    {
        if (player == null)
        {
            playerInSight = false;
            return;
        }
        
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // Check if player is within range
        if (distanceToPlayer > detectionRange)
        {
            playerInSight = false;
            return;
        }
        
        // Check if player is within field of view
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer > fieldOfView / 2f)
        {
            playerInSight = false;
            return;
        }
        
        // Check if there's a clear line of sight (no obstacles)
        if (Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstructionLayer))
        {
            playerInSight = false;
            return;
        }
        
        // Player is visible!
        playerInSight = true;
    }
    
    public bool CanSeePlayer()
    {
        return playerInSight;
    }
    
    // Gizmos for debugging
    void OnDrawGizmosSelected()
    {
        // Detection range
        Gizmos.color = playerInSight ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Field of view
        Vector3 leftBoundary = Quaternion.Euler(0, -fieldOfView / 2f, 0) * transform.forward * detectionRange;
        Vector3 rightBoundary = Quaternion.Euler(0, fieldOfView / 2f, 0) * transform.forward * detectionRange;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        
        // Line to player
        if (player != null)
        {
            Gizmos.color = playerInSight ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}