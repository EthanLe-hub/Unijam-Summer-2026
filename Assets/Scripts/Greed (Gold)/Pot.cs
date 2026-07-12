// Ethan Le (7/11/2026):
using System.Collections; // For IEnumerator. 
using UnityEngine;
using TMPro; 

/**
 * Script for handling Pot logic in the Greed minigame.
**/
public class Pot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinCount; 
    [SerializeField] TextMeshProUGUI coinReq; 

    int coinsCollected = 0;
    int coinsNeeded = 30;

    void Start()
    {
        coinCount.text = coinsCollected.ToString(); 
        coinReq.text = coinsNeeded.ToString(); 
    }

    public int GetCoinsCollected()
    {
        return coinsCollected;
    }

    public void AddCoin()
    {
        coinsCollected++; 

        coinCount.text = coinsCollected.ToString(); // Update coin count. 
    }

    public int GetCoinsNeeded()
    {
        return coinsNeeded; 
    }
}