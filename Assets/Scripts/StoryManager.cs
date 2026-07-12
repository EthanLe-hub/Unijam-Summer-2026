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

    private int currentIndex = 0; // Important to correspond the character line to the correct character art. 

    [SerializeField] float delayTime; // For any potential delays desired before showing the dialogue. 

    void Awake()
    {
        dialoguePanel = GameObject.FindGameObjectWithTag(panelTag); 
        if (dialoguePanel == null)
        {
            Debug.Log("Dialogue panel not found!"); 
        }

        playerScript = GameObject.FindGameObjectWithTag(playerTag).GetComponent<Player>(); 

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

            ShowCurrentLine(); 
        }
    }

    IEnumerator DelayTime(float seconds)
    {
        yield return new WaitForSeconds(seconds); // Delay time. 

        dialoguePanel.SetActive(true); 

        ShowCurrentLine(); 
    }

    void Update()
    {
        // Detect space key:
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            NextLine(); // Move onto next line of dialogue. 
        }
    }

    void ShowCurrentLine()
    {
        characterText.text = characterLines[currentIndex]; // Index 0 of the array of character dialogues. 

        characterArt.sprite = characterImages[currentIndex]; // Index 0 of the array of character art. 
    }

    void NextLine()
    {
        currentIndex++; // Shift index.

        if (currentIndex < characterLines.Length && currentIndex < characterImages.Length)
        {
            ShowCurrentLine(); 
        }
        else
        {
            playerScript.OnEnable(); // Re-enable player input again. 
            DisableDialogue(); // Turn off dialogue panel when done with dialogue. 
        } 
    }

    void DisableDialogue()
    {
        dialoguePanel.SetActive(false); 
    }
}