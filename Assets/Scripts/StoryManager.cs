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
    
    OverallGameManager overallGM; 

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
        overallGM = OverallGameManager.Instance;

        currentIndex = 0; 

        //playerScript = GameObject.FindGameObjectWithTag(playerTag).GetComponent<Player>(); 

        dialoguePanel.SetActive(false); // Turn off the Dialogue Panel first. 

        if (playerScript != null)
        {   
            playerScript.OnDisable(); // Disable other player inputs during dialogue. 
        }

        // After beating Lust:
        if (overallGM.lustComplete && !overallGM.lustStoryDone)
        {
            if (delayTime > 0f)
            {
                StartCoroutine(DelayTime(delayTime, overallGM.lustCharacterLines, overallGM.lustCharacterImages)); // Start coroutine if any delays needed before showing dialogue. 
            }
            else
            {
                StartDialogue(overallGM.lustCharacterLines, overallGM.lustCharacterImages); 
            }
            overallGM.lustStoryDone = true; // Story is playing (mark as complete).
        }
        // After beating Greed:
        else if (overallGM.greedComplete && !overallGM.greedStoryDone)
        {
            if (delayTime > 0f)
            {
                StartCoroutine(DelayTime(delayTime, overallGM.greedCharacterLines, overallGM.greedCharacterImages)); // Start coroutine if any delays needed before showing dialogue. 
            }
            else
            {
                StartDialogue(overallGM.greedCharacterLines, overallGM.greedCharacterImages); 
            }
            overallGM.greedStoryDone = true; // Story is playing (mark as complete).
        }
        // After beating Wrath:
        else if (overallGM.wrathComplete && !overallGM.wrathStoryDone)
        {
            if (delayTime > 0f)
            {
                StartCoroutine(DelayTime(delayTime, overallGM.wrathCharacterLines, overallGM.wrathCharacterImages)); // Start coroutine if any delays needed before showing dialogue. 
            }
            else
            {
                StartDialogue(overallGM.wrathCharacterLines, overallGM.wrathCharacterImages); 
            }
            overallGM.wrathStoryDone = true; // Story is playing (mark as complete).
        }
        // After beating Gluttony:
        else if (overallGM.gluttonyComplete && !overallGM.gluttonyStoryDone)
        {
            if (delayTime > 0f)
            {
                StartCoroutine(DelayTime(delayTime, overallGM.gluttonyCharacterLines, overallGM.gluttonyCharacterImages)); // Start coroutine if any delays needed before showing dialogue. 
            }
            else
            {
                StartDialogue(overallGM.gluttonyCharacterLines, overallGM.gluttonyCharacterImages); 
            }
            overallGM.gluttonyStoryDone = true; // Story is playing (mark as complete).
        }
        // After beating Envy:
        else if (overallGM.envyComplete && !overallGM.envyStoryDone)
        {
            if (delayTime > 0f)
            {
                StartCoroutine(DelayTime(delayTime, overallGM.envyCharacterLines, overallGM.envyCharacterImages)); // Start coroutine if any delays needed before showing dialogue. 
            }
            else
            {
                StartDialogue(overallGM.envyCharacterLines, overallGM.envyCharacterImages); 
            }
            overallGM.envyStoryDone = true; // Story is playing (mark as complete).
        }
        // After beating Sloth:
        else if (overallGM.slothComplete && !overallGM.slothStoryDone)
        {
            if (delayTime > 0f)
            {
                StartCoroutine(DelayTime(delayTime, overallGM.slothCharacterLines, overallGM.slothCharacterImages)); // Start coroutine if any delays needed before showing dialogue. 
            }
            else
            {
                StartDialogue(overallGM.slothCharacterLines, overallGM.slothCharacterImages); 
            }
            overallGM.slothStoryDone = true; // Story is playing (mark as complete).
        }
        // After beating Pride (the ending):
        else if (overallGM.prideComplete && !overallGM.prideStoryDone)
        {
            if (delayTime > 0f)
            {
                StartCoroutine(DelayTime(delayTime, overallGM.prideCharacterLines, overallGM.prideCharacterImages)); // Start coroutine if any delays needed before showing dialogue. 
            }
            else
            {
                StartDialogue(overallGM.prideCharacterLines, overallGM.prideCharacterImages); 
            }
            overallGM.prideStoryDone = true; // Story is playing (mark as complete).
        }
        else if (!isInMinigame && introComplete) // If we go back to the Hub Area after intro has already played. 
        {
            if (playerScript != null)
            {   
                playerScript.OnEnable(); // Re-enable other player inputs. 
            }
            return; // Gets out of Start() so intro does not play again every time you reload the Hub Area. 
        }
        else // Otherwise, default dialogue to play in this specific Scene (unique per minigame Scene):
        {
            if (delayTime > 0f)
            {
                StartCoroutine(DelayTime(delayTime, characterLines, characterImages)); // Start coroutine if any delays needed before showing dialogue. 
            }
            else
            {
                StartDialogue(characterLines, characterImages); 
            }
        }
    }

    IEnumerator DelayTime(float seconds, string[] characterLinesSet, Sprite[] characterImagesSet)
    {
        yield return new WaitForSeconds(seconds); // Delay time. 

        StartDialogue(characterLinesSet, characterImagesSet); 
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
                TransitionManager.Instance.TransitionToNextScene(hubSceneNumber); // Load the appropriate scene based on index. 
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