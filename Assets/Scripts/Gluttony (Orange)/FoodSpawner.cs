using System.Collections;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [Header("Food Prefabs")]
    [SerializeField] GameObject burgerPrefab;
    [SerializeField] GameObject drinkPrefab;
    [SerializeField] GameObject friesPrefab;
    
    [Header("Spawn Configuration")]
    [SerializeField] float spawnX = 10f;
    [SerializeField] float topLaneY = 2f;
    [SerializeField] float middleLaneY = 0f;
    [SerializeField] float bottomLaneY = -2f;
    
    [Header("Spawn Timing")]
    [SerializeField] float minSpawnDelay = 0.8f;
    [SerializeField] float maxSpawnDelay = 2f;
    
    [Header("Food Speed")]
    [SerializeField] float minFoodSpeed = 3f;
    [SerializeField] float maxFoodSpeed = 6f;
    
    Coroutine spawnCoroutine;
    bool isSpawning = false;
    
    public void BeginSpawning()
    {
        isSpawning = true;
        spawnCoroutine = StartCoroutine(SpawnLoop());
    }
    
    public void StopSpawning()
    {
        isSpawning = false;
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
        
        foreach (Food food in FindObjectsByType<Food>(FindObjectsSortMode.None))
        {
            Destroy(food.gameObject);
        }
    }
    
    IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(0.5f);
        
        while (isSpawning)
        {
            SpawnRandomFood();
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);
        }
    }
    
    void SpawnRandomFood()
    {
        FoodType randomType = (FoodType)Random.Range(0, 3);
        int randomLane = Random.Range(0, 3);
        
        GameObject prefab = GetPrefabForType(randomType);
        if (prefab == null) return;
        
        float yPos = GetLaneY(randomLane);
        Vector3 spawnPos = new Vector3(spawnX, yPos, 0f);
        
        GameObject foodObj = Instantiate(prefab, spawnPos, Quaternion.identity);
        Food food = foodObj.GetComponent<Food>();
        
        if (food != null)
        {
            float speed = Random.Range(minFoodSpeed, maxFoodSpeed);
            food.Initialize(randomType, speed);
        }
    }
    
    GameObject GetPrefabForType(FoodType type)
    {
        switch (type)
        {
            case FoodType.Burger: return burgerPrefab;
            case FoodType.Drink: return drinkPrefab;
            case FoodType.Fries: return friesPrefab;
            default: return burgerPrefab;
        }
    }
    
    float GetLaneY(int lane)
    {
        switch (lane)
        {
            case 0: return topLaneY;
            case 1: return middleLaneY;
            case 2: return bottomLaneY;
            default: return middleLaneY;
        }
    }
}