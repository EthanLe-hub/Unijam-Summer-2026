// Ethan Le (7/11/2026):
using System.Collections; // For IEnumerator. 
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; 
using TMPro; 

/**
 * Script to control the dialogue sequences (attached to the Dialogue Panel prefab).
**/
public class StoryManager : MonoBehaviour
{
    Player playerScript; // For disabling player inputs during dialogue sequence. 
    string playerTag = "Player"; 

    GameObject dialoguePanel; // Holds the background, character text, and character art. 
    string panelTag = "Dialogue Panel"; 

    TextMeshProUGUI characterText; // Displays the character's text.

    Image characterArt; // Displays the character art. 
    string imageTag = "Character Art"; 

    [TextArea(3,5)]
    [SerializeField] string[] characterLines; // Holds all of the dialogue by indices.
    [SerializeField] Sprite[] characterImages; // Holds all of the character art by indices. 

    string[] characterLinesToDisplay; // The official array dialogue being displayed. 
    Sprite[] characterImagesToDisplay; // The official character art being displayed. 

    private int currentIndex = 0; // Important to correspond the character line to the correct character art. 

    [SerializeField] float delayTime; // For any potential delays desired before showing the dialogue. 

    [SerializeField] bool isInMinigame = false; // Toggle between true or false if dialogue is happening in a minigame. 
    [SerializeField] Timer timer; // Assign in Unity Inspector. 

    void Awake()
    {
        dialoguePanel = GameObject.FindGameObjectWithTag(panelTag); 
        if (dialoguePanel == null)
        {
            Debug.Log("Dialogue panel not found!"); 
        }

        characterText = GetComponentInChildren<TextMeshProUGUI>(); 
        if (characterText == null)
        {
            Debug.Log("Text component not found!"); 
        }

        characterArt = GameObject.FindGameObjectWithTag(imageTag).GetComponent<Image>(); 
        if (characterArt == null)
        {
            Debug.Log("Character art component not found!"); 
        }
    }

    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag(playerTag).GetComponent<Player>(); 

        dialoguePanel.SetActive(false); // Turn off the Dialogue Panel first. 

        if (playerScript != null)
        {   
            playerScript.OnDisable(); // Disable other player inputs during dialogue. 
        }

        if (delayTime > 0f)
        {
            StartCoroutine(DelayTime(delayTime)); // Start coroutine if any delays needed before showing dialogue. 
        }
        else
        {
            dialoguePanel.SetActive(true); 

            StartDialogue(characterLines, characterImages); 
        }
    }

    IEnumerator DelayTime(float seconds)
    {
        yield return new WaitForSeconds(seconds); // Delay time. 

        dialoguePanel.SetActive(true); 

        StartDialogue(characterLines, characterImages); 
    }

    void Update()
    {
        // Detect space key:
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            NextLine(); // Move onto next line of dialogue. 
        }
    }

    public void StartDialogue(string[] characterLinesSet, Sprite[] characterImagesSet)
    {
        characterLinesToDisplay = characterLinesSet; // Set the official array dialogue being displayed. 
        characterImagesToDisplay = characterImagesSet; // Set the official character art being displayed. 

        ShowCurrentLine(); 
    }

    void ShowCurrentLine()
    {
        characterText.text = characterLinesToDisplay[currentIndex]; // Index 0 of the array of character dialogues. 

        characterArt.sprite = characterImagesToDisplay[currentIndex]; // Index 0 of the array of character art. 
    }

    void NextLine()
    {
        currentIndex++; // Shift index.

        if (currentIndex < characterLinesToDisplay.Length && currentIndex < characterImagesToDisplay.Length)
        {
            ShowCurrentLine(); 
        }
        else
        {
            playerScript.OnEnable(); // Re-enable player input again. 
            DisableDialogue(); // Turn off dialogue panel when done with dialogue. 

            if (isInMinigame)
            {
                if (timer != null)
                {
                    timer.BeginTimer(); // Start the timer after dialogue ends, if we are in a minigame. 
                }
            }
        } 
    }

    void DisableDialogue()
    {
        dialoguePanel.SetActive(false); 
    }
}