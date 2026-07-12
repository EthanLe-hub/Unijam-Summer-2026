using UnityEngine;
using TMPro;
using System.Collections.Generic;

public enum FoodType { Burger, Drink, Fries }

[System.Serializable]
public class FoodOrderItem
{
    public FoodType foodType;
    public int quantity;
}

public class GluttonyGameManager : MonoBehaviour
{
    [Header("Food Order Configuration")]
    [SerializeField] List<FoodOrderItem> foodOrder = new List<FoodOrderItem>();
    
    [Header("References")]
    [SerializeField] FoodSpawner spawner;
    [SerializeField] Timer timer;
    
    [Header("UI")]
    [SerializeField] TextMeshProUGUI orderDisplayText;
    [SerializeField] TextMeshProUGUI burgerCountText;
    [SerializeField] TextMeshProUGUI drinkCountText;
    [SerializeField] TextMeshProUGUI friesCountText;
    
    [SerializeField] GameObject endPanel;
    [SerializeField] TextMeshProUGUI endMessageText;
    
    int currentOrderIndex = 0;
    int currentItemCount = 0;
    bool gameActive = false;
    bool gameStarted = false;
    
    void Start()
    {
        endPanel.SetActive(false);
        UpdateOrderUI();
    }
    
    void Update()
    {
        if (!gameStarted && timer != null && timer.isTimerRunning)
        {
            gameStarted = true;
            gameActive = true;
            spawner.BeginSpawning();
        }
    }
    
    public bool TryEatFood(FoodType eatenType)
    {
        if (!gameActive) return false;
        
        if (currentOrderIndex >= foodOrder.Count)
            return false;
        
        FoodOrderItem currentRequired = foodOrder[currentOrderIndex];
        
        if (eatenType == currentRequired.foodType)
        {
            currentItemCount++;
            
            if (currentItemCount >= currentRequired.quantity)
            {
                currentOrderIndex++;
                currentItemCount = 0;
                
                if (currentOrderIndex >= foodOrder.Count)
                {
                    EndGame(true);
                }
            }
            
            UpdateOrderUI();
            return true;
        }
        else
        {
            EndGame(false);
            return false;
        }
    }
    
    void UpdateOrderUI()
    {
        int burgersNeeded = 0;
        int drinksNeeded = 0;
        int friesNeeded = 0;
        
        for (int i = currentOrderIndex; i < foodOrder.Count; i++)
        {
            FoodOrderItem item = foodOrder[i];
            int remaining = (i == currentOrderIndex) ? (item.quantity - currentItemCount) : item.quantity;
            
            switch (item.foodType)
            {
                case FoodType.Burger: burgersNeeded += remaining; break;
                case FoodType.Drink: drinksNeeded += remaining; break;
                case FoodType.Fries: friesNeeded += remaining; break;
            }
        }
        
        if (burgerCountText != null) burgerCountText.text = burgersNeeded + "x";
        if (drinkCountText != null) drinkCountText.text = drinksNeeded + "x";
        if (friesCountText != null) friesCountText.text = friesNeeded + "x";
            
        if (orderDisplayText != null)
        {
            if (currentOrderIndex < foodOrder.Count)
            {
                FoodOrderItem current = foodOrder[currentOrderIndex];
                int remaining = current.quantity - currentItemCount;
                orderDisplayText.text = "Eat " + remaining + "x " + current.foodType + "!";
            }
            else
            {
                orderDisplayText.text = "Order Complete!";
            }
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
            endMessageText.text = "GLUTTONY SATISFIED!";
        }
        else
        {
            endMessageText.text = "WRONG ORDER!\nGAME OVER";
        }
    }
    
    public void ReturnToHub()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Hub Area");
    }
}