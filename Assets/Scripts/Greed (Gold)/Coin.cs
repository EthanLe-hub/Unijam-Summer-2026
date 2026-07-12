// Ethan Le (7/11/2026):
using UnityEngine;

/** 
 * Script for the Greed minigame where the player must collect falling coins into a pot.
**/ 
public class Coin : MonoBehaviour
{
    string potTag = "Pot"; 

    Rigidbody2D rigidBody; 
    Pot pot; 
    
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        if (transform.position.y < -3.6f)
        {
            Destroy(gameObject); // Destroy the coin if not collected once it falls too far down. 
        }
    }

    public void SetFallSpeedMultiplier(float gravityMultiplier)
    {
        if (rigidBody != null)
        {
            rigidBody.gravityScale *= gravityMultiplier; // Multiply the Coin's gravitational fall to make it fall faster. 
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(potTag)) // If coin touches the pot, add it to the counter. 
        {
            pot = other.gameObject.GetComponent<Pot>(); 
            if (pot != null)
            {
                pot.AddCoin(); 
            }

            Destroy(gameObject); 
        }
    }
}