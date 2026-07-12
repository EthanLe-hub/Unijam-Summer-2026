using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] FoodType foodType;
    [SerializeField] float moveSpeed = 5f;
    
    float leftBoundary = -12f;
    bool isEaten = false;
    
    void Update()
    {
        if (isEaten) return;
        
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        
        if (transform.position.x < leftBoundary)
        {
            Destroy(gameObject);
        }
    }
    
    public void Initialize(FoodType type, float speed)
    {
        foodType = type;
        moveSpeed = speed;
    }
    
    public FoodType GetFoodType()
    {
        return foodType;
    }
    
    public void GetEaten()
    {
        isEaten = true;
        Destroy(gameObject);
    }
}