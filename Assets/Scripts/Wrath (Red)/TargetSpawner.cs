using System.Collections;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{


    [SerializeField] RectTransform spawnArea;

    [SerializeField] ClickTarget targetPrefab;

    [SerializeField] float targetLifetime = 1.2f;

    [SerializeField] Vector2 delayRange = new Vector2(0.1f, 0.3f);
    
    WrathGameManager gameManager;

    Coroutine loop;

    ClickTarget currentTarget;

    public void Init(WrathGameManager manager)
    {
        gameManager = manager;
    }
    
    public void BeginSpawning()
    {

        loop = StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        if (loop != null) StopCoroutine(loop);
        if (currentTarget != null) Destroy(currentTarget.gameObject);

    }
    
    IEnumerator SpawnLoop()
    {

        while (true)
        {
            // spawn new target
            currentTarget = Instantiate(targetPrefab, spawnArea);
            currentTarget.Init(gameManager, RandomPositionInArea());
            
            // Debug.Log("Spawned target, waiting " + targetLifetime + " seconds");

            yield return new WaitForSeconds(targetLifetime);

            if (currentTarget != null) 
            {

                Destroy(currentTarget.gameObject);
            }
            
            // small delay before next spawn
            float delay = Random.Range(delayRange.x, delayRange.y);
            // Debug.Log("Next spawn in: " + delay);
            yield return new WaitForSeconds(delay);
        }
    }

    
    Vector2 RandomPositionInArea()
    { 

        Rect r = spawnArea.rect;
        
        // offset so target doesnt clip edge
        float halfW = targetPrefab.GetComponent<RectTransform>().sizeDelta.x / 2f;
        float halfH = targetPrefab.GetComponent<RectTransform>().sizeDelta.y / 2f;

        float x = Random.Range(r.xMin + halfW, r.xMax - halfW);
        float y = Random.Range(r.yMin + halfH, r.yMax - halfH);
        
        
        return new Vector2(x, y);  
    }
}