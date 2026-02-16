using UnityEngine;

public class PlayerShootController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ShootAction shootAction;
    [SerializeField] private Animator animator;
    [SerializeField] private Camera playerCam; 
    
    [Header("Settings")]
    [SerializeField] private string shootAnimationTrigger = "attack";
    [SerializeField] private float maxShootRange = 100f;
    
    void Start()
    {
        if (shootAction == null) shootAction = GetComponent<ShootAction>();
        if (animator == null) animator = GetComponent<Animator>();
        if (playerCam == null) playerCam = Camera.main;
        
        // Debug checks
        if (shootAction == null)
        {
            Debug.LogError("ShootAction not found on Player!");
        }
        if (playerCam == null)
        {
            Debug.LogError("Camera not found!");
        }
    }
    
    void Update()
    {
        // Shoot when left mouse button is pressed (removed CanShoot check)
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    
    void Shoot()
    {
        if (shootAction == null)
        {
            Debug.LogError("Cannot shoot - ShootAction is null!");
            return;
        }
        
        // Play animation
        if (animator != null)
        {
            animator.SetTrigger(shootAnimationTrigger);
        }
        
        // Get where player is looking (center of screen)
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        
        Vector3 targetPoint;
        
        // If raycast hits something, shoot at that point
        if (Physics.Raycast(ray, out hit, maxShootRange))
        {
            targetPoint = hit.point;
            Debug.Log("Shooting at target: " + hit.collider.name);
        }
        else
        {
            // Otherwise shoot straight forward
            targetPoint = ray.GetPoint(maxShootRange);
            Debug.Log("Shooting straight ahead");
        }
        
        // Shoot at the target point
        shootAction.ShootAtPosition(targetPoint);
    }
}