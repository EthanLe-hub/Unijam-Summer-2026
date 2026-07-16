// Ethan Le (7/15/2026):
using System.Collections; // For IEnumerator.
using UnityEngine;
using UnityEngine.SceneManagement; 

/** 
 * Script so Unity Scenes fades between transitions.
**/ 
public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; } // Global static instance. 

    [SerializeField] CanvasGroup fadeCanvasGroup; // Assign in Unity Inspector.

    [SerializeField] float fadeDuration = 1f; // 1 second for the fade.

    bool isTransitioning = false; // True when new scene loads up to initiate transition. 
    float timePassed; // Keeps track of how long it takes to fade (compares against fadeDuration to know when fade is done).

    AsyncOperation asyncLoading; // Scene to load asynchronously. 

    void Awake()
    {
        if (Instance != null) // If a static instance of this script already exists, destroy the newly created copy. 
        {
            Destroy(gameObject); 
            return; 
        }

        Instance = this; // Otherwise, assign global static instance of this script for the first time.
        DontDestroyOnLoad(gameObject); 
    }

    void Start()
    {
        // First scene immediately has a transition fade:
        StartCoroutine(Fading(1, 0)); // startingAlpha = 1 (completely colored), endingAlpha = 0 (completely uncolored / invisible). 
    }

    // Every Unity Scene will call this function when loading a different Unity Scene: 
    public void TransitionToNextScene(int sceneNumber) 
    {
        if (isTransitioning)
        {
            return; // Prevents transitions from triggering twice at the same time. 
        }

        StartCoroutine(LoadNextScene(sceneNumber)); // Begin transition to next scene. 
    }

    // Function that loads the next scene with fades:
    IEnumerator LoadNextScene(int sceneNumber)
    {
        isTransitioning = true; // Mark as true so we do not accidentally have multiple transitions playing at once. 

        // (1) Do not allow any UI interactions during transition:
        fadeCanvasGroup.blocksRaycasts = true; 

        // (2) Fade to black (0 = invisible, 1 = completely colored):
        yield return StartCoroutine(Fading(0, 1)); 

        // (3) Load the next scene while screen is black:
        asyncLoading = SceneManager.LoadSceneAsync(sceneNumber); 

        // Let scene fully load first:
        while (!asyncLoading.isDone)
        {
            yield return null; 
        }

        // (4) Slowly fade out into next scene (aka, turn alpha to 0 to uncolor the black screen):
        yield return StartCoroutine(Fading(1, 0)); 

        // (5) Allow UI interactions since transition is now complete:
        fadeCanvasGroup.blocksRaycasts = false; 
        isTransitioning = false; // Transition is now done. 
    }

    // Function that fades the screen in and out:
    IEnumerator Fading(float startingAlpha, float endingAlpha)
    {
        timePassed = 0f; // Begin timer. 

        while (timePassed < fadeDuration) // Continue fade-in or fade-out until we reach the desired fadeDuration:
        {
            timePassed += Time.deltaTime; // Increment the time that has passed. 

            // Shift the alpha smoothly (from startingAlpha to endingAlpha): 
            fadeCanvasGroup.alpha = Mathf.Lerp(startingAlpha, endingAlpha, timePassed / fadeDuration); 

            yield return null; // Keeps the loop going (otherwise, it would end the function after the first millisecond).
        }

        fadeCanvasGroup.alpha = endingAlpha; // Once the desired fadeDuration is achieved, have the black screen be completely uncolored (new scene now opened up!). 
    }
}