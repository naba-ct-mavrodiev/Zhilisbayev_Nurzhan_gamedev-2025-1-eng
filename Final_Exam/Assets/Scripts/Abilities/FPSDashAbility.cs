// ============================================
// FPS Dash Ability
// ============================================
// PURPOSE: Quickly dash forward while respecting walls and collisions
// USAGE: Attach to FPS Player (must have CharacterController component)
// HOW IT WORKS: Uses CharacterController.Move() which stops at walls
// ============================================

using UnityEngine;
using UnityEngine.InputSystem;

// RequireComponent makes sure CharacterController exists on this object
[RequireComponent(typeof(CharacterController))]
public class FPSDashAbility : MonoBehaviour
{
    // ===== Input =====
    [Header("Input")]
    [Tooltip("The input action that triggers the dash (e.g., Shift key)")]
    public InputActionReference dashAction;

    // ===== Settings =====
    [Header("Settings")]
    [Tooltip("How far to dash (in meters)")]
    public float dashDistance = 5f;

    [Tooltip("How long the dash takes (in seconds)")]
    public float dashDuration = 0.2f;

    [Tooltip("Cooldown before you can dash again (in seconds)")]
    public float cooldown = 1f;

    // ===== Optional Feedback =====
    [Header("Feedback (Optional)")]
    [Tooltip("If assigned, this object will be spawned when dashing")]
    public GameObject dashEffect;

    // ===== Private Variables =====
    // Reference to the CharacterController component
    private CharacterController controller;

    // Reference to the camera for direction
    private Camera playerCamera;

    // Is a dash currently happening?
    private bool isDashing = false;

    // Cooldown tracking
    private float cooldownTimer = 0f;

    // Dash movement tracking
    private Vector3 dashDirection;
    private float dashTimer;
    private float dashSpeed;

    // ============================================
    // UNITY LIFECYCLE METHODS
    // ============================================

    private void Awake()
    {
        // Get the CharacterController on this object
        // This is the component that handles player movement and collisions
        controller = GetComponent<CharacterController>();

        // Find the camera
        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    private void OnEnable()
    {
        // Enable input and subscribe to the event
        if (dashAction != null)
        {
            dashAction.action.Enable();
            dashAction.action.performed += OnDashInput;
        }
    }

    private void OnDisable()
    {
        // Clean up: unsubscribe from events
        if (dashAction != null)
        {
            dashAction.action.performed -= OnDashInput;
            dashAction.action.Disable();
        }
    }

    private void Update()
    {
        // Handle cooldown timer
        // This counts down every frame
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Handle dash movement
        // If we're dashing, move the player each frame
        if (isDashing)
        {
            UpdateDash();
        }
    }

    // ============================================
    // INPUT HANDLING
    // ============================================

    private void OnDashInput(InputAction.CallbackContext context)
    {
        // Try to start a dash
        Dash();
    }

    // ============================================
    // THE ACTUAL ABILITY
    // ============================================

    [ContextMenu("Dash")]
    public void Dash()
    {
        // Check 1: Are we already dashing?
        if (isDashing)
        {
            Debug.Log("Already dashing!");
            return; // Exit early, can't dash while dashing
        }

        // Check 2: Is the cooldown still active?
        if (cooldownTimer > 0f)
        {
            Debug.Log("Dash on cooldown! Wait " + cooldownTimer.ToString("F1") + " seconds");
            return; // Exit early, still on cooldown
        }

        // All checks passed! Start the dash
        StartDash();
    }

    private void StartDash()
    {
        // Mark that we're now dashing
        isDashing = true;

        // Reset the dash timer
        dashTimer = 0f;

        // Calculate dash direction (where the player is looking)
        if (playerCamera != null)
        {
            // Get camera forward, but flatten it to horizontal
            // This prevents dashing into the ground or sky
            dashDirection = playerCamera.transform.forward;
            dashDirection.y = 0f; // Remove vertical component
            dashDirection.Normalize(); // Make it length 1
        }
        else
        {
            dashDirection = transform.forward;
        }

        // Calculate how fast we need to move
        // Speed = Distance / Time
        dashSpeed = dashDistance / dashDuration;

        // Spawn effect if we have one
        if (dashEffect != null)
        {
            Instantiate(dashEffect, transform.position, Quaternion.identity);
        }

        Debug.Log("Dash started!");
    }

    private void UpdateDash()
    {
        // Add to our timer
        dashTimer += Time.deltaTime;

        // Calculate how much to move this frame
        // Movement = Direction * Speed * Time
        Vector3 movement = dashDirection * dashSpeed * Time.deltaTime;

        // Use CharacterController.Move() - this respects collisions!
        // If we hit a wall, we stop (unlike transform.Translate)
        controller.Move(movement);

        // Check if dash is complete
        if (dashTimer >= dashDuration)
        {
            EndDash();
        }
    }

    private void EndDash()
    {
        // Mark that we're done dashing
        isDashing = false;

        // Start the cooldown
        cooldownTimer = cooldown;

        Debug.Log("Dash ended! Cooldown started.");
    }

    // ============================================
    // PUBLIC METHODS FOR TRIGGERS
    // ============================================

    // Check if we can dash (useful for UI feedback)
    public bool CanDash()
    {
        return !isDashing && cooldownTimer <= 0f;
    }

    // Get remaining cooldown (useful for UI)
    public float GetCooldownRemaining()
    {
        return Mathf.Max(0f, cooldownTimer);
    }
}

// ============================================
// HOW TO USE
// ============================================
// 1. Make sure your FPS Player has a CharacterController component
//
// 2. Create an Input Action:
//    - Go to your Input Actions asset
//    - Add a new action called "Dash"
//    - Bind it to a key (e.g., Left Shift)
//
// 3. Attach this script to your FPS Player
//
// 4. Assign the Input Action in the Inspector
//
// 5. Play and press the key!
//
// KEY DIFFERENCE FROM PhaseAbility:
// - PhaseAbility uses transform.Translate() = goes through walls
// - FPSDashAbility uses CharacterController.Move() = stops at walls
// ============================================
