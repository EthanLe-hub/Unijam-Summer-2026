// Ethan Le (7/11/2026):
using UnityEngine; 

/** 
 * Script for managing player movement:
**/
public class Player : MonoBehaviour
{
    InputSystem_Actions controls;

    Vector2 moveInput; // Holds direction of where the player will move. 

    bool jumpPressed; // Flag for when the player jumps (one press). 
    bool isCrouching; // Flag for when the player is crouching (continuous). 

    float speed = 0.1f; // Speed of the player. 

    Vector2 moveDirection; // Holds the official direction (with the applied speed) that the player will walk. 

    void Awake()
    {
        controls = new InputSystem_Actions(); // Initialize script that handles the player's input controls. 

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); // Set up "listener" for when the player moves (gets Vector2 of the direction). 
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero; // Set up "listener" for when the player stops moving (gets a zeroed Vector2, meaning stay still).

        controls.Player.Jump.performed += _ => jumpPressed = true; // If the player jumps, set the flag to true. 

        controls.Player.Crouch.performed += _ => isCrouching = true; // Set flag to true if player holds down on crouch. 
        controls.Player.Crouch.canceled += _ => isCrouching = false; // Set flag to false when player lets go of crouch. 
    }

    public void OnEnable() => controls.Enable(); // Function to turn on player controls. 
    public void OnDisable() => controls.Disable(); // Function to turn off player controls. 

    void Update() 
    {
        moveDirection = (Vector2) this.transform.position + (moveInput * speed); 

        this.transform.position = moveDirection; 
    }
}