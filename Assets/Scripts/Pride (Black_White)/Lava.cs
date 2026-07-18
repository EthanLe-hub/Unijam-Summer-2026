// Ethan Le (7/17/2026):
using UnityEngine;
using UnityEngine.UI;

/**
 * Script for the rising lava in the Lucifer (Pride) level (attached to the Lava GameObject).
**/
public class Lava : MonoBehaviour
{
    public float riseSpeed = 1.0f; // Speed of the lava rising. 
    public float maxHeight = 10.0f; // Max height of the lava. 
    private float currentHeight; // Current lava height (determined by placement in the Unity Scene). 

    string playerTag = "Player";
    string friendTag = "Friend"; 

    public bool isLavaRising = false; 
    public bool touchedLava = false; 
    
    void Start()
    {
        currentHeight = transform.position.y; // Get the y-position of the lava's placement from the Unity Scene. 
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        // If the player or the friend touches the lava, it is Game Over. 
        if (other.gameObject.CompareTag(playerTag) || other.gameObject.CompareTag(friendTag))
        {
            isLavaRising = false; // Keep the lava still upon Game Over. 
            touchedLava = true; // Player has touched the lava (flag that PrideGameManager.cs script will use to mark the end of the game).
        }
    }

    void Update()
    {
        // If the lava has reached the max height (or not activated), keep it still (don't continue rising it):
        if (currentHeight >= maxHeight || !isLavaRising)
        {
            return; // Exit the function. 
        }

        currentHeight += riseSpeed * Time.deltaTime; // Continuously raise the height of the lava over time based on rise speed.

        // Get the position of the lava and create a new Vector3 to update its y-position (mimics the rising of the lava):
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z); 
    }
}