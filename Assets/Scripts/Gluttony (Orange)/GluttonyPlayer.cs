using UnityEngine;

public class GluttonyPlayer : MonoBehaviour
{
    [Header("Lane Configuration")]
    [SerializeField] float topLaneY = 2f;
    [SerializeField] float middleLaneY = 0f;
    [SerializeField] float bottomLaneY = -2f;
    
    [Header("Movement")]
    [SerializeField] float laneChangeSpeed = 15f;
    
    [Header("References")]
    [SerializeField] GluttonyGameManager gameManager;
    
    int currentLane = 1;
    float targetY;
    
    InputSystem_Actions controls;
    Vector2 moveInput;
    bool canChangeLane = true;
    float cooldownTimer = 0f;
    float laneChangeCooldown = 0.15f;
    
    void Awake()
    {
        controls = new InputSystem_Actions();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }
    
    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();
    
    void Start()
    {
        currentLane = 1;
        targetY = middleLaneY;
        transform.position = new Vector3(transform.position.x, middleLaneY, transform.position.z);
    }
    
    void Update()
    {
        HandleLaneInput();
        MoveToTargetLane();
        
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
                canChangeLane = true;
        }
    }
    
    void HandleLaneInput()
    {
        if (!canChangeLane) return;
        
        if (moveInput.y > 0.5f && currentLane > 0)
        {
            currentLane--;
            UpdateTargetLane();
            StartCooldown();
        }
        else if (moveInput.y < -0.5f && currentLane < 2)
        {
            currentLane++;
            UpdateTargetLane();
            StartCooldown();
        }
    }
    
    void UpdateTargetLane()
    {
        switch (currentLane)
        {
            case 0: targetY = topLaneY; break;
            case 1: targetY = middleLaneY; break;
            case 2: targetY = bottomLaneY; break;
        }
    }
    
    void StartCooldown()
    {
        canChangeLane = false;
        cooldownTimer = laneChangeCooldown;
    }
    
    void MoveToTargetLane()
    {
        Vector3 pos = transform.position;
        pos.y = Mathf.Lerp(pos.y, targetY, laneChangeSpeed * Time.deltaTime);
        transform.position = pos;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Food food = other.GetComponent<Food>();
            if (food != null && gameManager != null)
            {
                gameManager.TryEatFood(food.GetFoodType());
                food.GetEaten();
            }
        }
    }
}// Gluttony (Orange) Minigame - Timer Bridge for StoryManager
using UnityEngine;
using TMPro;

public class GluttonyTimer : MonoBehaviour
{
    // These fields match what StoryManager expects from Timer
    public float timerLeft = 1f; // Dummy value, not used
    public bool isTimerRunning = false;
    
    [SerializeField] GluttonyGameManager gameManager;
    
    // Called by StoryManager when dialogue ends
    public void BeginTimer()
    {
        isTimerRunning = true;
        
        if (gameManager != null)
        {
            gameManager.StartGame();
        }
    }
    
    public float GetTimeLeft()
    {
        return timerLeft; // Always returns positive so game doesn't "end"
    }
}