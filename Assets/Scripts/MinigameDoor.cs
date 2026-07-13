// Ethan Le (7/11/2026):
using UnityEngine; 
using UnityEngine.InputSystem; 
using UnityEngine.SceneManagement; 

/**
 * Script for handling which Unity Scene to open when the player enters a minigame door.
**/
public class MinigameDoor : MonoBehaviour
{
    string playerTag = "Player"; 

    bool inFrontOfDoor = false; // To mark when the player is in front of the door. 

    [SerializeField] int sceneNumber; // Index of the Scene to load. 

    [SerializeField] GameplayUI gameplayUI; // To show indicator on how to enter the minigame doors. 

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            inFrontOfDoor = true; 

            if (inFrontOfDoor)
            {
                gameplayUI.ShowDoorButton(); 
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            inFrontOfDoor = false; 

            if (!inFrontOfDoor)
            {
                gameplayUI.HideDoorButton(); 
            }
        }
    }

    void Update()
    {
        if (inFrontOfDoor == true && Keyboard.current.xKey.wasPressedThisFrame)
        {
            if (sceneNumber < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
            {
                if (sceneNumber >= 0)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneNumber); // Load the appropriate scene based on index. 
                    //Debug.Log("Scene Number opened: " + sceneNumber); 
                }
            }
            else
            {
                Debug.LogError("Scene " + sceneNumber + " was not found in Build Settings!"); 
            }
        }
    }
}