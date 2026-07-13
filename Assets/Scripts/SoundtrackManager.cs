// Ethan Le (7/12/2026):
using UnityEngine;

/** 
 * Script to control soundtrack.
**/
public class SoundtrackManager : MonoBehaviour 
{
    private SoundtrackManager instance; // New soundtrack upon every scene reload. 

    void Awake()
    {
        if (instance == null) // Create a new static instance if game was loaded up for the first time.
        {
            instance = this; 
            //DontDestroyOnLoad(gameObject); // Do not restart the music between scene reloads. 
        }
/*
        else // If an instance already exists, do not create a duplicate (AKA, destroy the new one, and keep the old). 
        {
            Destroy(gameObject); 
        }
*/
    }
}