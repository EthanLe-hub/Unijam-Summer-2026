// Ethan Le (7/11/2026):
using Unity.Cinemachine; // For the camera that follows the playable entity. 
using UnityEngine; 
using UnityEngine.InputSystem; // For "Keyboard.current.someKey.wasPressedThisFrame" logic. 

/** 
 * Script for managing player (Isabel) movement:
**/
public class Player : MonoBehaviour
{
    InputSystem_Actions controls;
    Friend friendScript; // Only for the final level (Lucifer's minigame). 

    Vector2 moveInput; // Holds direction of where the player will move. 

    CinemachineCamera cinemachineCamera; // The camera that is focused on the playable entity. 

    bool jumpPressed; // Flag for when the player presses jump. 
    bool crouchPressed; // Flag for when the player presses crouch. 
    bool isCrouching; // Flag for when the player is crouching (continuous). 
    bool isGrounded; // Flag for when the player is on the ground. 
    public bool isPlayerControlled = true; // Switches to false when Diego (Friend.cs) is being controlled (only in Lucifer level). 

    [SerializeField] private LayerMask whatIsGround; // Mask assigned in Unity Inspector determining what layer is the ground.
    [SerializeField] private Transform groundCheck; // Assign GameObject that was placed below the player's feet that is used to check if player touched the ground. 
    [SerializeField] private Transform ceilingCheck; // Assign GameObject that was placed above the player's head that is used to check if player touched the ceiling. 

    // Player has two separate colliders: the top half and the bottom half.
    [SerializeField] Collider2D crouchDisableCollider; // This top half collider gets disabled when player crouches. 

    [SerializeField] float regularSpeed = 10f; // Default speed of the player. 
    float currentSpeed; // Player's current speed. 
    [SerializeField] float crouchSpeed = 0.5f; // Crouch speed is half the regular speed. 
    [SerializeField] float jumpForce = 8f; // How high the player should jump. 

    float facingX; // For the direction the player is facing (positive = right, negative = left). 

    Animator animator; 
    Rigidbody2D rigidBody; // Holds the official component that moves the player. 

    void Awake()
    {
        controls = new InputSystem_Actions(); // Initialize script that handles the player's input controls. 

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); // Set up "listener" for when the player moves (gets Vector2 of the direction). 
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero; // Set up "listener" for when the player stops moving (gets a zeroed Vector2, meaning stay still).

        controls.Player.Jump.performed += _ => jumpPressed = true; // If the player jumps, set the flag to true. 

        controls.Player.Crouch.performed += _ => crouchPressed = true; // Set flag to true if player holds down on crouch. 
        controls.Player.Crouch.canceled += _ => crouchPressed = false; // Set flag to false when player lets go of crouch. 

        friendScript = GetComponent<Friend>(); 
        if (friendScript != null) // Only if the Friend.cs script is attached to the Player.cs's GameObject (meaning it is the final level against Lucifer).
        {
            friendScript.controls = controls; // Set Diego's controls to be the same as Isabel's. 
        }

        animator = GetComponent<Animator>(); 
        rigidBody = GetComponent<Rigidbody2D>(); 
    }

    void Start()
    {
        isPlayerControlled = true; 
        if (friendScript != null) // Only found in the final level (Lucifer's minigame). 
        {
            friendScript.isFriendControlled = false; 
        }
    }

    public void OnEnable() => controls?.Enable(); // Function to turn on player controls. 
    public void OnDisable() => controls?.Disable(); // Function to turn off player controls. 

    void Update() 
    {
        if (facingX == 0f) // Default. 
        {
            facingX = 1f; 
        }

        // If Isabel (this) is currently being controlled and player presses E key, swap controls to Diego:
        if (friendScript != null && Keyboard.current.eKey.wasPressedThisFrame) 
        {
            isPlayerControlled = !isPlayerControlled; // Opposite of current flag. 
            friendScript.isFriendControlled = !isPlayerControlled; // Opposite of the flag that was just assigned in previous line. 
            
            ResetInputs(); // Ensures neither character gets stuck running/sliding. 

            friendScript.ResetInputs(); 

            if (isPlayerControlled) // Follow Isabel if she is being controlled. 
            {
                cinemachineCamera.Follow = transform; 
            }
            else if (friendScript.isFriendControlled) // Follow Diego if he is being controlled.
            {
                cinemachineCamera.Follow = friendScript.gameObject.transform; 
            }

            return; // Ends execution for this frame. 
        }

        if (isPlayerControlled) // Only allow input and movement if currently controlling the player. 
        {
            if (moveInput.x > 0f) // If the player is moving right, 
            {
                facingX = 1f; // then the player should face the right.
            }
            else if (moveInput.x < 0f) // If the player is moving left, 
            {
                facingX = -1f; // then the player should face the left. 
            }

            Vector3 s = transform.localScale; // Normalize the vector based on character scale. 
            float ax = Mathf.Abs(s.x); // Absolute value of the current x direction. 
            transform.localScale = new Vector3(facingX * ax, s.y, s.z); // Officially shift the sprite's x direction, as needed. 

            if (animator != null)
            {
                animator.SetFloat("speed", Mathf.Abs(moveInput.x)); 
            }
        }
    }

    // Helper method to stop movement slide when swapping:
    private void ResetInputs()
    {
        moveInput = Vector2.zero; // Sets magnitude to 0. 
        jumpPressed = false; 
        crouchPressed = false; 
        if (rigidBody != null)
        {
            rigidBody.linearVelocity = new Vector2(0, rigidBody.linearVelocity.y); // No horizontal movement, just drop entity straight down as needed. 
        }

        if (animator != null)
        {
            animator.SetFloat("speed", Mathf.Abs(moveInput.x)); // Ensure the animation resets to idle if player was running during swap. 
            animator.SetBool("crouching", false); // Ensure the animation resets to idle if player was crouching during swap. 
        }
    }

    void FixedUpdate() // We check if the player is touching the ground here:
    {
        if (isPlayerControlled) // Only allow input and movement if currently controlling the player. 
        {
            isGrounded = false; 

            // groundCheck.position -> the center of the circle marker that checks if itself is overlapping a GameObject with the Layer "Ground". 
            // 0.2f -> the radius of the circle marker. 
            // whatIsGround -> the Layer that the circle marker should be checking for. 
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f, whatIsGround); 
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject) // Ensures the player GameObject is not accidentally self-detected.
                {
                    isGrounded = true; 
                }
            }

            if (!isGrounded && Mathf.Abs(rigidBody.linearVelocity.y) > 0.1f)
            {
                jumpPressed = false; 
            }

            HandleMovement(); // Then handle movement accordingly based on if player is touching the ground or not. 
        }
    }

    void HandleMovement()
    {
        if (isPlayerControlled) // Only allow input and movement if currently controlling the player. 
        {
            if (crouchPressed) // Player holds down crouch button = crouching. 
            {
                isCrouching = true; 
            }
            else // Player not holding down crouch button but head is heading ceiling, crouch anyway. 
            {
                if (isGrounded) // Ensure the player is on the ground, otherwise game will make player crouch if player bumps head into ceiling. 
                {
                    isCrouching = Physics2D.OverlapCircle(ceilingCheck.position, 0.2f, whatIsGround); 
                }
            }

            // Jump if the player is both on the ground AND the jump key was pressed:
            if (isGrounded && jumpPressed)
            {
                isGrounded = false; // Player is off the ground. 

                // Create new vector for vertical movement:
                rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce); 
                jumpPressed = false; // Turn off flag so the player does not keep executing the jump command.
            }

            currentSpeed = regularSpeed; // Default is player's regular speed. 

            if (isCrouching) // When player is crouching. 
            {
                currentSpeed *= crouchSpeed; // Reduces player speed. 

                crouchDisableCollider.enabled = false; // Reduces the player hitbox. 

                if (animator != null)
                {
                    animator.SetBool("crouching", true); // Set Animator flag of "crouching" to true. 
                    animator.SetFloat("speed", 0); // Pretend the speed is 0 so the walk animation does not override crouch animation. 
                }
            }
            else // When player stops crouching. 
            {
                crouchDisableCollider.enabled = true; // Increase player hitbox again. 

                if (animator != null)
                {
                    animator.SetBool("crouching", false); // Set Animator flag of "crouching" to false. 
                }
            }

            // Create new vector for horizontal movement: 
            rigidBody.linearVelocity = new Vector2(moveInput.x * currentSpeed, rigidBody.linearVelocity.y); 
        }
    }
}