// Ethan Le (7/11/2026):
using UnityEngine;

/**
 * Game Manager to handle Greed minigame logic.
**/
public class GreedGameManager : MonoBehaviour
{
    [SerializeField] Timer timer; // Assign in Unity Inspector. 

    [SerializeField] Pot potScript; // Assign in Unity Inspector. 

    [TextArea(3,5)]
    [SerializeField] string[] victoryCharacterLines; // Holds all of the dialogue by indices.
    [SerializeField] Sprite[] victoryCharacterImages; // Holds all of the character art by indices. 

    [TextArea(3,5)]
    [SerializeField] string[] lostCharacterLines; // Holds all of the dialogue by indices.
    [SerializeField] Sprite[] lostCharacterImages; // Holds all of the character art by indices. 

    [SerializeField] StoryManager storyManager; // Assign in Unity Inspector. 

    bool dialogueComplete = false; 

    void Update()
    {
        if (!dialogueComplete)
        {
            // Victory (player has collected required coin amount):
            if (potScript.GetCoinsCollected() >= potScript.GetCoinsNeeded() && timer.GetTimeLeft() > 0f)
            {
                dialogueComplete = true; 
                OverallGameManager.Instance.greedComplete = true; // Greed game has been complete. 
                OverallGameManager.Instance.TryUnlockFinal(); // Try to unlock the final level.
                storyManager.StartDialogue(victoryCharacterLines, victoryCharacterImages); 
            }
            // Defeat (player did not collect enough coins in time):
            else if (potScript.GetCoinsCollected() < potScript.GetCoinsNeeded() && timer.GetTimeLeft() <= 0f)
            {
                dialogueComplete = true; 
                storyManager.StartDialogue(lostCharacterLines, lostCharacterImages); 
            }
        }
    }
}