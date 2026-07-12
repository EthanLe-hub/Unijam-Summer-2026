// Ethan Le (7/11/2026):
using System.Collections; // For IEnumerator. 
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 
using TMPro; 

/**
 * Script to control the dialogue sequences (attached to the Dialogue Panel prefab).
**/
public class StoryManager : MonoBehaviour
{
    [SerializeField] Player playerScript; // For disabling player inputs during dialogue sequence. 
    
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

    static bool introComplete = false; 

    [SerializeField] bool isBeginningCutscene = false; 
    [SerializeField] int hubSceneNumber = 1; 

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
        currentIndex = 0; 

        //playerScript = GameObject.FindGameObjectWithTag(playerTag).GetComponent<Player>(); 

        dialoguePanel.SetActive(false); // Turn off the Dialogue Panel first. 

        if (!isInMinigame && introComplete) // If we go back to the Hub Area after intro has already played. 
        {
            if (playerScript != null)
            {   
                playerScript.OnEnable(); // Re-enable other player inputs. 
            }
            return; // Gets out of Start() so intro does not play again every time you reload the Hub Area. 
        }

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
            StartDialogue(characterLines, characterImages); 
        }
    }

    IEnumerator DelayTime(float seconds)
    {
        yield return new WaitForSeconds(seconds); // Delay time. 

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

        if (playerScript != null)
        {   
            playerScript.OnDisable(); // Disable other player inputs during dialogue. 
        }

        currentIndex = 0; 

        dialoguePanel.SetActive(true); 

        ShowCurrentLine(); 
    }

    void ShowCurrentLine()
    {
        characterText.text = characterLinesToDisplay[currentIndex]; // Index 0 of the array of character dialogues. 

        characterArt.sprite = characterImagesToDisplay[currentIndex]; // Index 0 of the array of character art. 
    }

    void NextLine()
    {
        if (characterLinesToDisplay == null || characterImagesToDisplay == null)
        {
            return; 
        }

        currentIndex++; // Shift index.

        if (currentIndex < characterLinesToDisplay.Length && currentIndex < characterImagesToDisplay.Length)
        {
            ShowCurrentLine(); 
        }
        else
        {
            if (playerScript != null)
            {   
                playerScript.OnEnable(); // Re-enable player input again. 
            }
            DisableDialogue(); // Turn off dialogue panel when done with dialogue. 

            if (isBeginningCutscene) // Load hub scene if this was the beginning cutscene. 
            {
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(hubSceneNumber); // Load the appropriate scene based on index. 
            }
            else if (!introComplete && !isBeginningCutscene)
            {
                introComplete = true; 
            }

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
        currentIndex = 0; 
        dialoguePanel.SetActive(false); 
    }
}