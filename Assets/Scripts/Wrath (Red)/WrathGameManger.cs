using UnityEngine;
using TMPro;

public class WrathGameManager : MonoBehaviour
{
    [SerializeField] int clicksRequired = 300;
    [SerializeField] float timeLimit = 60f;
    
    [SerializeField] TargetSpawner spawner;

    [SerializeField] TextMeshProUGUI clickCounterText;
    [SerializeField] TextMeshProUGUI timerText;

    [SerializeField] GameObject endPanel;
    [SerializeField] TextMeshProUGUI endMessageText;

    int currentClicks;
    float timeRemaining;
    bool gameActive;
    
    void Start()
    {
        timeRemaining = timeLimit;
        gameActive = true;
        endPanel.SetActive(false);

        spawner.Init(this);

        spawner.BeginSpawning();
        
        UpdateClickUI();
        UpdateTimerUI();
        
        // Debug.Log("Wrath minigame started! Need " + clicksRequired + " clicks in " + timeLimit + " seconds");
    }
    
    void Update()
    {
        if (!gameActive) return;
        
        timeRemaining -= Time.deltaTime;
        UpdateTimerUI();

        if (timeRemaining <= 0f)
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

    void UpdateTimerUI()
    {
        int t = Mathf.Max(0, Mathf.CeilToInt(timeRemaining));
        int mins = t / 60;
        int secs = t % 60;
        timerText.text = mins + ":" + secs.ToString("00");
    }
    
    public void ReturnToHub()
    {
        // Debug.Log("Returning to hub");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Hub Area");
    }
}