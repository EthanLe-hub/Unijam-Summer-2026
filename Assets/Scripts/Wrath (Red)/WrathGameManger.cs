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
        
        if (won)
        {
            endMessageText.text = "WRATH CONQUERED!";
            // Debug.Log("Player won!");
        }
        else
        {
            endMessageText.text = "FAILED...";
            // Debug.Log("Player lost");
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