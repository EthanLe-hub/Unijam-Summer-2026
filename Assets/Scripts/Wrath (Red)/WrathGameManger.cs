using UnityEngine;
using TMPro;

public class WrathGameManager : MonoBehaviour
{
    [SerializeField] int clicksRequired = 300;
    
    [SerializeField] TargetSpawner spawner;
    [SerializeField] Timer timer;

    [SerializeField] TextMeshProUGUI clickCounterText;

    [SerializeField] GameObject endPanel;
    [SerializeField] TextMeshProUGUI endMessageText;

    int currentClicks;
    bool gameActive = false;
    bool gameStarted = false;

    bool dialogueComplete = false; 

    [TextArea(3,5)]
    [SerializeField] string[] victoryCharacterLines; // Holds all of the dialogue by indices.
    [SerializeField] Sprite[] victoryCharacterImages; // Holds all of the character art by indices. 

    [TextArea(3,5)]
    [SerializeField] string[] lostCharacterLines; // Holds all of the dialogue by indices.
    [SerializeField] Sprite[] lostCharacterImages; // Holds all of the character art by indices. 

    [SerializeField] StoryManager storyManager; // Assign in Unity Inspector. 
    
    void Start()
    {
        endPanel.SetActive(false);
        spawner.Init(this);
        UpdateClickUI();
        
        // Debug.Log("Wrath minigame ready, waiting for dialogue to finish...");
    }
    
    void Update()
    {
        // Wait for Timer to start (triggered by StoryManager after dialogue ends)
        if (!gameStarted && timer != null && timer.isTimerRunning)
        {
            gameStarted = true;
            gameActive = true;
            spawner.BeginSpawning();
            // Debug.Log("Dialogue done, game starting!");
        }
        
        if (!gameActive) return;

        // Check if time ran out
        if (timer.timerLeft <= 0f)
        {
            // Debug.Log("Time up! Final clicks: " + currentClicks);
            EndGame(currentClicks >= clicksRequired);
        }
    }

    public void RegisterClick()
    {
        if (!gameActive) return;

        currentClicks++;
        UpdateClickUI();

        // Debug.Log("Click registered: " + currentClicks + "/" + clicksRequired);
        
        if (currentClicks >= clicksRequired)
        {
            EndGame(true);
        }
    }
    
    void EndGame(bool won)
    {
        gameActive = false;
        timer.isTimerRunning = false;
        spawner.StopSpawning();

        endPanel.SetActive(true);
        
        if (!dialogueComplete)
        {
            if (won)
            {
                dialogueComplete = true; 
                endMessageText.text = "WRATH CONQUERED!";
                OverallGameManager.Instance.wrathComplete = true; 
                OverallGameManager.Instance.TryUnlockFinal(); // Try to unlock the final level. 
                storyManager.StartDialogue(victoryCharacterLines, victoryCharacterImages); 
                // Debug.Log("Player won!");
            }
            else
            {
                dialogueComplete = true; 
                endMessageText.text = "FAILED...";
                storyManager.StartDialogue(lostCharacterLines, lostCharacterImages); 
                // Debug.Log("Player lost");
            }
        }
    }
    
    void UpdateClickUI()
    {
        clickCounterText.text = currentClicks + " / " + clicksRequired;
    }
    
    public void ReturnToHub()
    {
        // Debug.Log("Returning to hub");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Hub Area");
    }
}