// Ethan Le (7/11/2026):
using System.Collections; // For IEnumerator. 
using UnityEngine;

/**
 * Script to instantiate a coin, which drops faster as the game continues.
**/
public class CoinSpawn : MonoBehaviour
{
    [SerializeField] Transform spawnArea; // Big rectangle Transform component at the top of the screen where the coins will drop from. 
    [SerializeField] Timer timer; // Assign in Unity. 

    [SerializeField] GameObject coinPrefab; // Assign in Unity. 

    [SerializeField] float leftmostX = -8f; // Furthest left of the screen. 
    [SerializeField] float rightmostX = 8f; // Furthest right of the screen. 

    float secondsPassed = 0f; // Counts how many seconds of the game have passed. 
    float scalar = 0.05f; // Multiplier to adjust secondsPassed in relation to graviational multiplier. 
    float gravityMultiplier; 

    float startingSpawnTime = 4f; // Original spawn time. 
    float spawnTimer; // Time to spawn the coins (gets shorter as the game does on). 
    float calculatedDelay; // Rate that a coin spawns.

    void Start()
    {
        spawnTimer = startingSpawnTime; 
    }

    void Update()
    {
        if (timer == null || !timer.isTimerRunning)
        {
            return; // Do not spawn coin yet if timer is not running. 
        }

        if (timer.timerLeft > 0f)
        {
            secondsPassed = (60f - timer.timerLeft); // Calculates the number of seconds that have passed.  

            calculatedDelay = timer.timerLeft * scalar; // Rate that a coin spawns. 

            if (calculatedDelay < 0.75f) // Cap is 0.75 seconds per coin spawn. 
            {
                calculatedDelay = 0.75f; 
            }

            spawnTimer -= Time.deltaTime; // Count down the spawn timer (until it hits 0, which is when we spawn another coin). 

            gravityMultiplier = secondsPassed * scalar; // Increase the gravity multiplier based on time left (cap at 0.75 seconds). 

            if (spawnTimer <= 0f) // Once the spawn timer hits 0 seconds, it is time to spawn another coin. 
            {
                SpawnCoin();

                spawnTimer = calculatedDelay; // Restart the cycle of the delay until the next coin is supposed to spawn. 
            }
        }
    }

    void SpawnCoin()
    {
        Vector2 spawnPos = new Vector2(Random.Range(leftmostX, rightmostX), spawnArea.transform.position.y); // Create random spawn point at the top of the screen. 

        // Create instance of the Coin prefab:
        GameObject coinInstance = Instantiate(coinPrefab, spawnPos, Quaternion.identity); 

        // Get the Coin.cs script of the coinInstance:
        Coin coinScript = coinInstance.GetComponent<Coin>(); 

        if (coinScript != null)
        {
            coinScript.SetFallSpeedMultiplier(gravityMultiplier); // Set the gravity fall speed of the new coin instance.
        }
    }

}