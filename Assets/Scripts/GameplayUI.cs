// Ethan Le (7/12/2026):
using System.Collections; // For IEnumerator. 
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/** 
 * Script for handling any UI that shows up during gameplay (button press indicators, etc.):
**/
public class GameplayUI : MonoBehaviour
{
    /* Door Indicator Panel (Xkey press): */
    [SerializeField] GameObject doorIndicatorPanel; // Holds the "X Press Image". 
    [SerializeField] Image currentXKeyImage; // Image component that flips between "xKey" and "xKeyPressed" sprites. 
    [SerializeField] Sprite xKey; // Non-pressed Xkey sprite. 
    [SerializeField] Sprite xKeyPressed; // Pressed Xkey sprite. 
    private Coroutine currentCoroutine; // The current coroutine that is running. 

    void Awake()
    {
        if (xKey == null)
        {
            Debug.Log("Xkey button image not assigned!"); 
        }

        if (xKeyPressed == null)
        {
            Debug.Log("Xkey button press image not assigned!"); 
        }
    }

    void Start()
    {
        currentXKeyImage.sprite = xKey; // Initial frame is the default Xkey (not pressed). 
        HideDoorButton(); 
    }

    public void ShowDoorButton()
    {
        if (!doorIndicatorPanel.activeSelf)
        {
            doorIndicatorPanel.SetActive(true); 

            currentCoroutine = StartCoroutine(timeTilSwap()); // Begin timer to swap frames (new instance upon every call). 
        }
    }

    public void HideDoorButton()
    {
        if (doorIndicatorPanel != null)
        {
            if (doorIndicatorPanel.activeSelf)
            {
                doorIndicatorPanel.SetActive(false); 

                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine); // Stop timer of frame swap. 
                    currentCoroutine = null; 
                }
            }
        }
    }

    // Timer to swap frames in GameplayUI.cs:
    IEnumerator timeTilSwap()
    {
        // Continuously flip between frames every 2 seconds: 
        while (doorIndicatorPanel.activeSelf) 
        {
            yield return new WaitForSeconds(1.5f); // Two seconds before switching frames. 

            SwitchFrames();  
        }
    }

    public void SwitchFrames()
    {
        if (currentXKeyImage.sprite == xKey)
        {
            currentXKeyImage.sprite = xKeyPressed; // Switch to pressed Xkey image if current frame is the non-pressed Xkey image. 
        }

        else if (currentXKeyImage.sprite == xKeyPressed)
        {
            currentXKeyImage.sprite = xKey; // Switch to non-pressed Xkey image if current frame is the pressed Xkey image. 
        }
    }
}