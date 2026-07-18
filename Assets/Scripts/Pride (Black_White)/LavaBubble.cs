// Ethan Le (7/17/2026):
using System.Collections; // For IEnumerator. 
using UnityEngine;

/**
 * Script for Lava Bubble properties and logic when touched.
**/
public class LavaBubble : MonoBehaviour
{
    Lava lavaScript; // To retrieve the isLavaRising and touchedLava flags. 

    string playerTag = "Player";

    string friendTag = "Friend"; 

    Rigidbody2D rigidBody; // The component that moves the Lava Bubble itself. 

    void Awake() 
    {
        rigidBody = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component from this prefab GameObject. 
    }

    public void InitializeLavaScript (Lava lava)
    {
        lavaScript = lava; 
    }

    void Update()
    {
        if (transform.position.y < -4f)
        {
            Destroy(gameObject); // Destroy the coin if not collected once it falls too far down. 
        }
    }

    public void SetFallSpeedMultiplier(float gravityMultiplier)
    {
        if (rigidBody != null)
        {
            rigidBody.gravityScale *= gravityMultiplier; // Multiply the Lava Bubble's gravitational fall to make it fall faster. 
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If the player or the friend touches the lava, it is Game Over. 
        if (other.gameObject.CompareTag(playerTag) || other.gameObject.CompareTag(friendTag))
        {
            lavaScript.isLavaRising = false; // Keep the lava still upon Game Over. 
            lavaScript.touchedLava = true; // Player has touched the lava (flag that PrideGameManager.cs script will use to mark the end of the game).
        }
    }
}