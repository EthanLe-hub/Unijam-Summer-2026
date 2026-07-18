// Ethan Le (7/17/2026):
using TMPro; 
using UnityEngine;
using UnityEngine.UI;

/** 
 * Script to manage any minigame's record (versatile).
**/
public class Record : MonoBehaviour
{
    [SerializeField] public float recordTime; // Total time to record. 
    public bool isRecordRunning = false; // Timer does not start yet.

    [SerializeField] TextMeshProUGUI timerText; // Text to display the time. 

    void Update()
    {
        if (isRecordRunning)
        {
            recordTime += Time.deltaTime; // Increment each second. 
            ShowTimer(recordTime); // Function to display the total time. 
        }
    }

    public void BeginRecord()
    {
        isRecordRunning = true; 
    }

    public void StopRecord()
    {
        isRecordRunning = false; 
    }

    void ShowTimer(float displayedTime)
    {
        displayedTime++; // To show the previous second correctly. 

        float minutes = Mathf.FloorToInt(displayedTime / 60); // Total seconds / 60 = number of exact minutes.
        float seconds = Mathf.FloorToInt(displayedTime % 60); // Total seconds % 60 = remainder = number of seconds. 
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Set the timer text. 
    }

    public float GetTotalTime()
    {
        return recordTime; 
    }
}