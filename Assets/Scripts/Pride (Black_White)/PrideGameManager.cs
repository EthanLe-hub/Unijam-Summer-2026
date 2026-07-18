// Ethan Le (7/17/2026):
using UnityEngine; 

/** 
 * Game Manager to handle Pride minigame logic.
**/
public class PrideGameManager : MonoBehaviour
{
    [SerializeField] Record timerRecord; // Assign in Unity Inspector (to stop the recorder). 

    [TextArea(3,5)]
    [SerializeField] string[] lostCharacterLines; // Holds all of the dialogue by indices.
    [SerializeField] Sprite[] lostCharacterImages; // Holds all of the character art by indices. 

    [SerializeField] StoryManager storyManager; // Assign in Unity Inspector. 

    bool dialogueComplete = false; 

    [SerializeField] Lava lavaScript; // Needed so the game knows when the player or the friend has touched lava. 

    void Update()
    {
        if (!dialogueComplete)
        {
            // Victory (player has escaped through the final exit door (marked by ExitDoor.cs script)):
            if (OverallGameManager.Instance.prideComplete)
            {
                timerRecord.StopRecord(); // Stop the timer from continuing. 
                dialogueComplete = true; 
            }
            // Defeat (player or the friend touched lava):
            else if (lavaScript.touchedLava)
            {
                timerRecord.StopRecord(); // Stop the timer from continuing. 
                dialogueComplete = true; 
                storyManager.StartDialogue(lostCharacterLines, lostCharacterImages); 
                storyManager.failedLucifer = true; // Flag so the game knows when to take the player back to the hub after defeat dialogue plays. 
            }
        }
    }
}