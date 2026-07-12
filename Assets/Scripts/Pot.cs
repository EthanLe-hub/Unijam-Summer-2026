// Ethan Le (7/11/2026):
using System.Collections; // For IEnumerator. 
using UnityEngine;

/**
 * Script for handling Pot logic in the Greed minigame.
**/
public class Pot : MonoBehaviour
{
    int coinsCollected = 0;
    int coinsNeeded = 100;
    [SerializeField] Timer timer; // Assign in Unity Inspector. 

    [TextArea(3,5)]
    [SerializeField] string[] characterLines; // Holds all of the dialogue by indices.

    [SerializeField] Sprite[] characterImages; // Holds all of the character art by indices. 

    [SerializeField] StoryManager storyManager; // Assign in Unity Inspector. 

    public int GetCoinsCollected()
    {
        return coinsCollected;
    }

    public void AddCoin()
    {
        coinsCollected++; 
    }

    public int GetCoinsNeeded()
    {
        return coinsNeeded; 
    }

    void Update()
    {
        if (coinsCollected == coinsNeeded && timer.GetTimeLeft() > 0f)
        {
            storyManager.StartDialogue(characterLines, characterImages); 
        }
    }
}