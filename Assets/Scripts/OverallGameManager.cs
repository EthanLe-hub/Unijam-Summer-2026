// Ethan Le (7/14/2026):
using UnityEngine;
using UnityEngine.UI; 

/** 
 * Static Game Manager to handle global variables (like trial completion).
**/
public class OverallGameManager : MonoBehaviour
{
    public static OverallGameManager Instance; // Global variable holding this script, which any outside script can access. 

    // Flags for when minigames are complete and their story completions:
    public bool lustComplete = false; // Lust minigame completion flag. 
    public bool lustStoryDone = false; 
    [TextArea(3,5)]
    [SerializeField] public string[] lustCharacterLines; // Holds all of the dialogue by indices.
    [SerializeField] public Sprite[] lustCharacterImages; // Holds all of the character art by indices. 

    public bool greedComplete = false; // Greed minigame completion flag. 
    public bool greedStoryDone = false; 
    [SerializeField] public string[] greedCharacterLines; // Holds all of the dialogue by indices.
    [SerializeField] public Sprite[] greedCharacterImages; // Holds all of the character art by indices. 

    public bool wrathComplete = false; // Wrath minigame completion flag. 
    public bool wrathStoryDone = false;
    [SerializeField] public string[] wrathCharacterLines; // Holds all of the dialogue by indices.
    [SerializeField] public Sprite[] wrathCharacterImages; // Holds all of the character art by indices. 

    public bool gluttonyComplete = false; // Gluttony minigame completion flag. 
    public bool gluttonyStoryDone = false; 
    [SerializeField] public string[] gluttonyCharacterLines; // Holds all of the dialogue by indices.
    [SerializeField] public Sprite[] gluttonyCharacterImages; // Holds all of the character art by indices. 

    public bool envyComplete = false; // Envy minigame completion flag. 
    public bool envyStoryDone = false; 
    [SerializeField] public string[] envyCharacterLines; // Holds all of the dialogue by indices.
    [SerializeField] public Sprite[] envyCharacterImages; // Holds all of the character art by indices. 

    public bool slothComplete = false; // Sloth minigame completion flag. 
    public bool slothStoryDone = false; 
    [SerializeField] public string[] slothCharacterLines; // Holds all of the dialogue by indices.
    [SerializeField] public Sprite[] slothCharacterImages; // Holds all of the character art by indices. 

    public bool prideComplete = false; // Pride minigame completion flag. 
    public bool prideStoryDone = false; 
    [SerializeField] public string[] prideCharacterLines; // Holds all of the dialogue by indices.
    [SerializeField] public Sprite[] prideCharacterImages; // Holds all of the character art by indices. 

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); // If global instance already exists, get rid of this newly created copy.
            return;
        }

        Instance = this; // Otherwise, create the global instance for the first time. 
        DontDestroyOnLoad(gameObject); // Script persists between scene loads. 
    }
}