// Ethan Le (7/17/2026):
using UnityEngine;
using UnityEngine.UI;

/** 
 * Script to handle the Title Screen.
**/
public class TitleScreen : MonoBehaviour
{
    [SerializeField] Button playButton; 
    [SerializeField] Button creditsButton; 
    int introSceneNumber = 1; 

    [SerializeField] GameObject creditsPanel; 
    [SerializeField] Button closeCreditsButton; 

    void Start()
    {
        playButton.onClick.RemoveAllListeners();
        creditsButton.onClick.RemoveAllListeners(); 
        closeCreditsButton.onClick.RemoveAllListeners(); 

        playButton.onClick.AddListener(PlayPressed);
        creditsButton.onClick.AddListener(CreditsPressed); 
        closeCreditsButton.onClick.AddListener(CloseCredits); 

        creditsPanel.SetActive(false); // Credits panel initially off. 
    }

    void PlayPressed()
    {
        TransitionManager.Instance.TransitionToNextScene(introSceneNumber); // Start the game (it is Scene 1 in the Build Settings). 
    }

    void CreditsPressed()
    {
        playButton.gameObject.SetActive(false); 
        creditsButton.gameObject.SetActive(false); 

        creditsPanel.SetActive(true); // Show credits panel. 
    }

    void CloseCredits()
    {
        playButton.gameObject.SetActive(true); 
        creditsButton.gameObject.SetActive(true); 

        creditsPanel.SetActive(false); // Close the credits panel. 
    }
}