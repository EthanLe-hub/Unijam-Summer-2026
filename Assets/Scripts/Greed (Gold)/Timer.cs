// Ethan Le (7/11/2026):
using TMPro; 
using UnityEngine;
using UnityEngine.UI;

/** 
 * Script to manage any minigame's timer (versatile).
**/
public class Timer : MonoBehaviour
{
    [SerializeField] public float timerLeft = 60f; // Number of seconds. 
    public bool isTimerRunning = false; // Timer does not start yet.

    [SerializeField] TextMeshProUGUI timerText; // Text to display the time. 

    void Update()
    {
        if (isTimerRunning)
        {
            if (timerLeft > 0) // Count down the timer if it has not hit 0 seconds yet.
            {
                timerLeft -= Time.deltaTime; // Decrement each second. 
                ShowTimer(timerLeft); // Function to display the remaining time. 
            }
            else
            {
                timerLeft = 0f; 
                isTimerRunning = false; 
            }
        }
    }

    public void BeginTimer()
    {
        isTimerRunning = true; 
    }

    void ShowTimer(float displayedTime)
    {
        displayedTime++; // To show the previous second correctly. 

        float minutes = Mathf.FloorToInt(displayedTime / 60); // Total seconds / 60 = number of exact minutes.
        float seconds = Mathf.FloorToInt(displayedTime % 60); // Total seconds % 60 = remainder = number of seconds. 
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Set the timer text. 
    }

    public float GetTimeLeft()
    {
        return timerLeft; 
    }
}