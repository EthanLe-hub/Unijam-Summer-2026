// Ethan Le (7/17/2026):
using System.Collections; // For IEnumerator. 
using UnityEngine;

/**
 * Script to instantiate a lava bubble, which drops faster as the game continues.
**/
public class LavaBubbleSpawn : MonoBehaviour
{
    [SerializeField] Transform spawnArea; // Small rectangle Transform component at the top of the screen where the lava bubbles will drop from. 
    [SerializeField] Record timerRecord; // Assign in Unity. 

    [SerializeField] GameObject lavaBubblePrefab; // Assign in Unity. 
    [SerializeField] Lava lavaScript; // Assign in Unity so it can be passed into every spawned LavaBubble.cs instance. 

    float secondsPassed = 0f; // Counts how many seconds of the game have passed. 
    float scalar = 0.05f; // Multiplier to adjust secondsPassed in relation to graviational multiplier. 
    float gravityMultiplier; 

    [SerializeField] float startingSpawnTime = 4f; // Original spawn time. 
    float spawnTimer; // Time to spawn the lava bubbles (gets shorter as the game does on). 
    float calculatedDelay; // Rate that a lava bubble spawns.

    void Start()
    {
        spawnTimer = startingSpawnTime; 
    }

    void Update()
    {
        if (timerRecord == null || !timerRecord.isRecordRunning)
        {
            return; // Do not spawn lava bubble yet if timer is not running. 
        }

        if (timerRecord.recordTime > 0f)
        {
            secondsPassed = timerRecord.recordTime; // Get the number of seconds that have passed.  

            calculatedDelay = (60f - timerRecord.recordTime) * scalar; // Rate that a lava bubble spawns. 

            if (calculatedDelay < 0.75f) // Cap is 0.75 seconds per coin spawn. 
            {
                calculatedDelay = 0.75f; 
            }

            spawnTimer -= Time.deltaTime; // Count down the spawn timer (until it hits 0, which is when we spawn another coin). 

            gravityMultiplier = secondsPassed * scalar; // Increase the gravity multiplier based on time left (cap at 0.75 seconds). 

            if (spawnTimer <= 0f) // Once the spawn timer hits 0 seconds, it is time to spawn another coin. 
            {
                SpawnLavaBubble();

                spawnTimer = calculatedDelay; // Restart the cycle of the delay until the next coin is supposed to spawn. 
            }
        }
    }

    void SpawnLavaBubble()
    {
        Vector2 spawnPos = new Vector2(spawnArea.transform.position.x, spawnArea.transform.position.y); // Create spawn point at the top of the screen. 

        // Create instance of the Lava Bubble prefab (contains the LavaBubble.cs script):
        GameObject bubbleInstance = Instantiate(lavaBubblePrefab, spawnPos, Quaternion.identity); 

        // Get the LavaBubbleSpawn.cs script of the bubbleInstance:
        LavaBubble bubbleScript = bubbleInstance.GetComponent<LavaBubble>(); 

        if (bubbleScript != null)
        {
            bubbleScript.SetFallSpeedMultiplier(gravityMultiplier); // Set the gravity fall speed of the new lava bubble instance.

            bubbleScript.InitializeLavaScript(lavaScript); // Give the Lava.cs script reference to the newly created LavaBubble.cs instance. 
        }
    }
}