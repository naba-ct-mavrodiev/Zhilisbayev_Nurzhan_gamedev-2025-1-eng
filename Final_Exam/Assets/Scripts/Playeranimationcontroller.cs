using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Assign your Magician_RIO_Unity model's Animator component")]
    public Animator animator;
    
    [Tooltip("Assign the parent capsule with CharacterController")]
    public CharacterController characterController;
    
    [Header("Animation Settings")]
    [Tooltip("Speed threshold to detect movement")]
    public float movementThreshold = 0.1f;
    
    [Header("Attack Settings")]
    [Tooltip("Cooldown between attacks in seconds")]
    public float attackCooldown = 0.5f;
    
    private bool isAttacking = false;
    private float lastAttackTime = 0f;
    private Vector3 lastPosition;
    private string currentState = "idle";
    private bool isInitialized = false;
    
    void Start()
    {
        // Auto-find components if not assigned
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
            if (animator == null)
            {
                Debug.LogError("No Animator found! Please assign the Animator component.");
            }
        }
        
        if (characterController == null)
        {
            characterController = GetComponentInParent<CharacterController>();
            if (characterController == null)
            {
                Debug.LogError("No CharacterController found! Please assign the CharacterController component.");
            }
        }
        
        // Start with idle animation
        if (animator != null)
        {
            animator.SetTrigger("idle");
        }
    }
    
    void Update()
    {
        // Initialize position tracking on first frame
        if (!isInitialized)
        {
            lastPosition = transform.position;
            isInitialized = true;
            return; // Skip first frame to avoid false movement detection
        }
        
        HandleMovementAnimation();
        HandleAttackInput();
        HandleJumpAnimation();
    }
    
    void HandleMovementAnimation()
    {
        if (characterController == null || animator == null) return;
        
        // Calculate current speed
        Vector3 currentPosition = transform.position;
        float speed = ((currentPosition - lastPosition).magnitude) / Time.deltaTime;
        lastPosition = currentPosition;
        
        // Check if player is moving
        bool isMoving = speed > movementThreshold;
        
        // Check if Shift is being held
        bool isShiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        
        // Check movement direction using input
        float verticalInput = Input.GetAxis("Vertical");
        bool isMovingBackward = verticalInput < -0.1f; // S key pressed
        bool isMovingForward = verticalInput > 0.1f;   // W key pressed
        
        // Determine the new state
        string newState = "idle";
        
        if (isMoving)
        {
            if (isMovingBackward)
            {
                // Moving backward
                if (isShiftHeld)
                {
                    newState = "run_backward";
                }
                else
                {
                    newState = "walk_backward";
                }
            }
            else
            {
                // Moving forward or strafing
                if (isShiftHeld)
                {
                    newState = "run";
                }
                else
                {
                    newState = "walk";
                }
            }
        }
        else
        {
            newState = "idle";
        }
        
        // Only trigger animation if state changed
        if (newState != currentState)
        {
            animator.SetTrigger(newState);
            currentState = newState;
        }
    }
    
    void HandleAttackInput()
    {
        if (animator == null) return;
        
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            // Check cooldown
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                TriggerAttack();
            }
        }
    }
    
    void TriggerAttack()
    {
        animator.SetTrigger("attack");
        isAttacking = true;
        lastAttackTime = Time.time;
        
        // Reset attacking flag after animation duration
        // Adjust this delay to match your attack animation length
        Invoke("ResetAttack", 0.5f);
    }
    
    void ResetAttack()
    {
        isAttacking = false;
    }
    
    void HandleJumpAnimation()
    {
        if (characterController == null || animator == null) return;
        
        // Detect jump input
        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            animator.SetTrigger("jump");
        }
    }
}