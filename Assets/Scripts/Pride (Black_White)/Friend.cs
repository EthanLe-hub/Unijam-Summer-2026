// Ethan Le (7/15/2026):
using UnityEngine; 
using UnityEngine.InputSystem; // For "Keyboard.current.someKey.wasPressedThisFrame" logic. 

/** 
 * Script to control Diego (Isabel's friend).
**/
public class Friend : MonoBehaviour
{   // This script gets attached to the same GameObject as Player.cs in ONLY the Pride level, but the moveable components (animator and rigidbody)
    // will be assigned by tag since Diego (the friend) has to be a different entity than Isabel (the player). 

    public InputSystem_Actions controls; // Player.cs will assign it from its class. 
    Player playerScript; 

    Vector2 moveInput; // Holds direction of where the player will move. 

    bool jumpPressed; // Flag for when the player presses jump. 
    bool crouchPressed; // Flag for when the player presses crouch. 
    bool isCrouching; // Flag for when the player is crouching (continuous). 
    bool isGrounded; // Flag for when the player is on the ground. 
    public bool isFriendControlled = false; // False is when Isabel (Player.cs) is being controlled (only in Lucifer level). 

    [SerializeField] private LayerMask whatIsGround; // Mask assigned in Unity Inspector determining what layer is the ground.
    [SerializeField] private Transform groundCheck; // Assign GameObject that was placed below the player's feet that is used to check if player touched the ground. 
    [SerializeField] private Transform ceilingCheck; // Assign GameObject that was placed above the player's head that is used to check if player touched the ceiling. 

    // Player has two separate colliders: the top half and the bottom half.
    [SerializeField] Collider2D crouchDisableCollider; // This top half collider gets disabled when player crouches. 

    [SerializeField] float regularSpeed = 10f; // Default speed of the player. 
    float currentSpeed; // Player's current speed. 
    [SerializeField] float crouchSpeed = 0.5f; // Crouch speed is half the regular speed. 
    [SerializeField] float jumpForce = 14f; // How high the player should jump. 

    float facingX; // For the direction the player is facing (positive = right, negative = left). 

    Animator animator; 
    GameObject friendObj; // Need to grab its Transform component so it faces appropriately when the player controls Diego. 
    Rigidbody2D rigidBody; // Holds the official component that moves the player. 
    string friendTag = "Friend"; 

    void Awake()
    {
        // Diego's moveable components are in a different GameObject (since Friend.cs has to be attached to Player.cs's GameObject).
        // So find these components by tag instead. 
        friendObj = GameObject.FindGameObjectWithTag(friendTag); // Get Diego's GameObject. 
        
        if (friendObj != null)
        {
            animator = friendObj.GetComponent<Animator>(); 
            rigidBody = friendObj.GetComponent<Rigidbody2D>(); 
        }
    }

    void Start()
    {
        if (controls != null)
        {
            controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); // Set up "listener" for when the player moves (gets Vector2 of the direction). 
            controls.Player.Move.canceled += ctx => moveInput = Vector2.zero; // Set up "listener" for when the player stops moving (gets a zeroed Vector2, meaning stay still).

            controls.Player.Jump.performed += _ => jumpPressed = true; // If the player jumps, set the flag to true. 

            controls.Player.Crouch.performed += _ => crouchPressed = true; // Set flag to true if player holds down on crouch. 
            controls.Player.Crouch.canceled += _ => crouchPressed = false; // Set flag to false when player lets go of crouch. 

            playerScript = GetComponent<Player>(); // Player.cs and Friend.cs scripts are in the same GameObject. 

            isFriendControlled = false; 
        }
    }

    // Helper method to stop movement slide when swapping:
    public void ResetInputs()
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
            animator.SetFloat("speed", Mathf.Abs(moveInput.x)); // Ensure animation resets to idle if Friend was running during swap. 
            animator.SetBool("crouching", false); // Ensure animation resets to idle if Friend was crouching during swap. 
        }
    }

    void Update() 
    {
        if (facingX == 0f) // Default. 
        {
            facingX = 1f; 
        }

        if (isFriendControlled) // Only allow input and movement if currently controlling the friend. 
        {
            if (moveInput.x > 0f) // If the player is moving right, 
            {
                facingX = 1f; // then the player should face the right.
            }
            else if (moveInput.x < 0f) // If the player is moving left, 
            {
                facingX = -1f; // then the player should face the left. 
            }

            Vector3 s = friendObj.transform.localScale; // Normalize the vector based on character scale. 
            float ax = Mathf.Abs(s.x); // Absolute value of the current x direction. 
            friendObj.transform.localScale = new Vector3(facingX * ax, s.y, s.z); // Officially shift the sprite's x direction, as needed. 

            if (animator != null)
            {
                animator.SetFloat("speed", Mathf.Abs(moveInput.x)); 
            }
        }
    }

    void FixedUpdate() // We check if the player is touching the ground here:
    {
        if (isFriendControlled) // Only allow input and movement if currently controlling the friend. 
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
        if (isFriendControlled) // Only allow input and movement if currently controlling the friend. 
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