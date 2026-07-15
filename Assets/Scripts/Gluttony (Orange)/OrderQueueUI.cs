using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderQueueUI : MonoBehaviour
{
    [Header("Food Sprites")]
    [SerializeField] Sprite burgerSprite;
    [SerializeField] Sprite drinkSprite;
    [SerializeField] Sprite friesSprite;
    
    [Header("Queue Configuration")]
    [SerializeField] Transform queueContainer; // Parent object for queue items
    [SerializeField] GameObject queueItemPrefab; // UI Image prefab
    [SerializeField] int maxVisibleItems = 5; // How many to show at once
    [SerializeField] float itemSpacing = 80f; // Pixels between items
    [SerializeField] float slideSpeed = 300f; // Pixels per second
    
    List<FoodType> fullOrder = new List<FoodType>();
    List<GameObject> queueImages = new List<GameObject>();
    int currentIndex = 0;
    
    public void InitializeQueue(List<FoodOrderItem> foodOrder)
    {
        fullOrder.Clear();
        
        // Flatten the order into individual items
        foreach (FoodOrderItem item in foodOrder)
        {
            for (int i = 0; i < item.quantity; i++)
            {
                fullOrder.Add(item.foodType);
            }
        }
        
        currentIndex = 0;
        RefreshQueueDisplay();
    }
    
    void RefreshQueueDisplay()
    {
        // Clear existing images
        foreach (GameObject img in queueImages)
        {
            Destroy(img);
        }
        queueImages.Clear();
        
        // Create new queue images
        int itemsToShow = Mathf.Min(maxVisibleItems, fullOrder.Count - currentIndex);
        
        for (int i = 0; i < itemsToShow; i++)
        {
            int orderIndex = currentIndex + i;
            if (orderIndex >= fullOrder.Count) break;
            
            GameObject item = Instantiate(queueItemPrefab, queueContainer);
            Image img = item.GetComponent<Image>();
            
            if (img != null)
            {
                img.sprite = GetSpriteForType(fullOrder[orderIndex]);
            }
            
            // Position from left to right
            RectTransform rect = item.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(i * itemSpacing, 0);
            
            queueImages.Add(item);
        }
    }
    
    public void ConsumeFirstItem()
    {
        if (currentIndex >= fullOrder.Count) return;
        
        currentIndex++;
        
        // Animate the first item out, then refresh
        if (queueImages.Count > 0)
        {
            StartCoroutine(SlideOutFirstItem());
        }
    }
    
    System.Collections.IEnumerator SlideOutFirstItem()
    {
        if (queueImages.Count == 0) yield break;
        
        GameObject firstItem = queueImages[0];
        RectTransform firstRect = firstItem.GetComponent<RectTransform>();
        
        // Slide first item to the left and fade out
        Image firstImg = firstItem.GetComponent<Image>();
        float elapsed = 0f;
        float duration = 0.2f;
        Vector2 startPos = firstRect.anchoredPosition;
        Vector2 endPos = startPos + Vector2.left * itemSpacing;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            firstRect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            
            if (firstImg != null)
            {
                Color c = firstImg.color;
                c.a = 1f - t;
                firstImg.color = c;
            }
            
            // Slide remaining items left
            for (int i = 1; i < queueImages.Count; i++)
            {
                RectTransform rect = queueImages[i].GetComponent<RectTransform>();
                Vector2 targetPos = new Vector2((i - 1) * itemSpacing, 0);
                rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, targetPos, t);
            }
            
            yield return null;
        }
        
        // Destroy the first item and refresh
        Destroy(firstItem);
        queueImages.RemoveAt(0);
        
        // Add new item at the end if more remain
        int nextIndex = currentIndex + queueImages.Count;
        if (nextIndex < fullOrder.Count && queueImages.Count < maxVisibleItems)
        {
            GameObject newItem = Instantiate(queueItemPrefab, queueContainer);
            Image img = newItem.GetComponent<Image>();
            
            if (img != null)
            {
                img.sprite = GetSpriteForType(fullOrder[nextIndex]);
            }
            
            RectTransform rect = newItem.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2((maxVisibleItems) * itemSpacing, 0); // Start off-screen right
            
            queueImages.Add(newItem);
            
            // Animate sliding in
            StartCoroutine(SlideInNewItem(rect, (queueImages.Count - 1) * itemSpacing));
        }
    }
    
    System.Collections.IEnumerator SlideInNewItem(RectTransform rect, float targetX)
    {
        float elapsed = 0f;
        float duration = 0.2f;
        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = new Vector2(targetX, 0);
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }
        
        rect.anchoredPosition = endPos;
    }
    
    Sprite GetSpriteForType(FoodType type)
    {
        switch (type)
        {
            case FoodType.Burger: return burgerSprite;
            case FoodType.Drink: return drinkSprite;
            case FoodType.Fries: return friesSprite;
            default: return burgerSprite;
        }
    }
    
    public FoodType? GetCurrentRequired()
    {
        if (currentIndex < fullOrder.Count)
            return fullOrder[currentIndex];
        return null;
    }
    
    public bool IsOrderComplete()
    {
        return currentIndex >= fullOrder.Count;
    }
}