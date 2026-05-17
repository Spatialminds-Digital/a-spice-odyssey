using System;
using System.Collections;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private OrderService orderService;
    [SerializeField] private QuickTimeEvent quickTimeEvent;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("Countdown Settings")]
    [SerializeField] private float countdownDuration = 3f;

    [Header("Order Settings")]
    [SerializeField] private float initialOrderInterval = 8f;
    [SerializeField] private float minimumOrderInterval = 3f;
    [SerializeField] private float intervalDecreaseRate = 0.1f;
    [SerializeField] private int maxActiveOrders = 5;

    [Header("Difficulty Settings")]
    [SerializeField] private int baseScoreForComplexity = 50;
    [SerializeField] private float complexityExponent = 2f;
    [SerializeField] private int maxComplexity = 5;

    [Header("QTE Settings")]
    [SerializeField] private float initialGreenZoneSize = 0.4f;
    [SerializeField] private float minimumGreenZoneSize = 0.1f;
    [SerializeField] private float initialQTESpeed = 1f;
    [SerializeField] private float maximumQTESpeed = 3f;

    [Header("Enemy Difficulty Settings")]
    [SerializeField] private float initialMainEnemySpeed = 2f;
    [SerializeField] private float maximumMainEnemySpeed = 5f;

    [Header("Lives Settings")]
    [SerializeField] private int initialLives = 3;

    [Header("Penalty Settings")]
    [SerializeField] private int basePenalty = 10;
    [SerializeField] private float penaltyScoreMultiplier = 0.05f;

    // Game State
    public enum GameState { Idle, Countdown, Playing, Paused, GameOver }
    private GameState _currentState = GameState.Idle;
    public GameState CurrentState => _currentState;

    // Score & Lives
    private int _score;
    private int _lives;
    private int _activeOrderCount;

    // Difficulty tracking
    private float _currentOrderInterval;
    private int _currentComplexity = 1;
    private Coroutine _orderSpawnCoroutine;

    // Cached components
    private PlayerMovement _playerMovement;
    public PlayerMovement playerMovement => _playerMovement;
    private PlayerInteractor _playerInteractor;

    // Cached WaitForSeconds
    private WaitForSecondsRealtime _countdownWait;
    private WaitForSeconds _orderSpawnWait;

    // Events
    public static event Action<int> OnCountdownTick;
    public static event Action OnGameStarted;
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnLivesChanged;
    public static event Action OnGameOver;
    public static event Action<bool> OnGamePaused;
    public static event Action<int> OnComplexityChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Cache components
        if (playerObject != null)
        {
            _playerMovement = playerObject.GetComponent<PlayerMovement>();
            _playerInteractor = playerObject.GetComponent<PlayerInteractor>();
        }

        // Cache WaitForSeconds
        _countdownWait = new WaitForSecondsRealtime(1f);
    }

    private void OnEnable()
    {
        if (orderService != null)
        {
            orderService.OnOrderComplete += HandleOrderComplete;
            orderService.OnWrongOrder += HandleWrongOrder;
            orderService.OnNewOrderCreated += HandleNewOrder;
            orderService.OnOrderRemoved += HandleOrderRemoved;
        }
    }

    private void OnDisable()
    {
        if (orderService != null)
        {
            orderService.OnOrderComplete -= HandleOrderComplete;
            orderService.OnWrongOrder -= HandleWrongOrder;
            orderService.OnNewOrderCreated -= HandleNewOrder;
            orderService.OnOrderRemoved -= HandleOrderRemoved;
        }
    }

    private void Start()
    {
        SetPlayerControlEnabled(false);
        
        
        StartGame();
    }

    public void StartGame()
    {
        if (_currentState != GameState.Idle && _currentState != GameState.GameOver) return;

        ResetGameState();
        StartCoroutine(CountdownRoutine());
    }

    private void ResetGameState()
    {
        _score = 0;
        _lives = initialLives;
        _currentOrderInterval = initialOrderInterval;
        _currentComplexity = 1;
        _activeOrderCount = 0;

        if (orderService != null)
            orderService.ClearAllOrders();

        OnScoreChanged?.Invoke(_score);
        OnLivesChanged?.Invoke(_lives);
        OnComplexityChanged?.Invoke(_currentComplexity);

        UpdateQTEDifficulty();
    }

    private IEnumerator CountdownRoutine()
    {
        _currentState = GameState.Countdown;
        SetPlayerControlEnabled(false);

        int countdown = Mathf.CeilToInt(countdownDuration);

        for (int i = countdown; i >= 0; i--)
        {
            OnCountdownTick?.Invoke(i);
            yield return _countdownWait;
        }

        BeginGameplay();
    }

    private void BeginGameplay()
    {
        _currentState = GameState.Playing;
        SetPlayerControlEnabled(true);
        OnGameStarted?.Invoke();

        _orderSpawnCoroutine = StartCoroutine(OrderSpawnRoutine());
    }

    private IEnumerator OrderSpawnRoutine()
    {
        // Create first order immediately
        CreateNewOrder();

        while (_currentState == GameState.Playing)
        {
            _orderSpawnWait = new WaitForSeconds(_currentOrderInterval);
            yield return _orderSpawnWait;

            if (_currentState != GameState.Playing) break;
            if (_activeOrderCount >= maxActiveOrders) continue;

            CreateNewOrder();
        }
    }

    private void CreateNewOrder()
    {
        if (orderService == null) return;
        orderService.CreateOrder(_currentComplexity);
    }

    private void HandleNewOrder(Order order)
    {
        _activeOrderCount++;
    }

    private void HandleOrderRemoved(Order order)
    {
        _activeOrderCount = Mathf.Max(0, _activeOrderCount - 1);
    }

    private void HandleOrderComplete(Order order)
    {
        _activeOrderCount = Mathf.Max(0, _activeOrderCount - 1);

        int orderScore = CalculateOrderScore(order.recipe);
        AddScore(orderScore);

        EffectService.Instance.ShowMessage($"+ {orderScore} kill!", Color.turquoise, 1);
    }

    private void HandleWrongOrder(Order order)
    {
        int penalty = CalculatePenalty();
        SubtractScore(penalty);
        EffectService.Instance.ShowMessage($"- {penalty} wrong spices!!", Color.indianRed, 1);
        AudioService.Instance.PlayError();
       // RemoveLife();
    }

    private int CalculateOrderScore(Recipe recipe)
    {
        if (recipe == null || recipe.recipeItems == null) return 0;

        int totalValue = 0;
        foreach (var recipeItem in recipe.recipeItems)
        {
            if (recipeItem.item != null)
            {
                totalValue += recipeItem.item.itemValue * recipeItem.count;
            }
        }
        return totalValue;
    }

    private int CalculatePenalty()
    {
        return basePenalty + Mathf.FloorToInt(_score * penaltyScoreMultiplier);
    }

    private void AddScore(int amount)
    {
        _score += amount;
        OnScoreChanged?.Invoke(_score);
        UpdateDifficulty();
    }

    private void SubtractScore(int amount)
    {
        _score = Mathf.Max(0, _score - amount);
        OnScoreChanged?.Invoke(_score);
    }

    private void UpdateDifficulty()
    {
        // Reverse exponential complexity: early levels come fast, later levels need more score
        // Level thresholds: 50, 150, 350, 750... (baseScore * (exponent^(level-1) - 1) / (exponent - 1))
        int newComplexity = CalculateComplexityFromScore(_score);
        if (newComplexity != _currentComplexity)
        {
            _currentComplexity = newComplexity;
            OnComplexityChanged?.Invoke(_currentComplexity);
        }

        // Update order interval based on complexity progress
        float complexityProgress = (_currentComplexity - 1f) / (maxComplexity - 1f);
        float intervalReduction = complexityProgress * (initialOrderInterval - minimumOrderInterval);
        _currentOrderInterval = initialOrderInterval - intervalReduction;

        // Update QTE difficulty
        UpdateQTEDifficulty();
    }

    private int CalculateComplexityFromScore(int score)
    {
        // Each level requires exponentially more points
        // Level 2: baseScore (50)
        // Level 3: baseScore + baseScore*exp (150)
        // Level 4: baseScore + baseScore*exp + baseScore*exp^2 (350)
        int cumulativeScore = 0;
        for (int level = 2; level <= maxComplexity; level++)
        {
            int scoreForThisLevel = Mathf.RoundToInt(baseScoreForComplexity * Mathf.Pow(complexityExponent, level - 2));
            cumulativeScore += scoreForThisLevel;

            if (score < cumulativeScore)
            {
                return level - 1;
            }
        }
        return maxComplexity;
    }

    private void UpdateQTEDifficulty()
    {
        // Calculate difficulty progress (0 to 1) based on complexity
        float difficultyProgress = (_currentComplexity - 1f) / (maxComplexity - 1f);

        // Update QTE difficulty
        if (quickTimeEvent != null)
        {
            // Green zone shrinks as difficulty increases
            float greenZoneSize = Mathf.Lerp(initialGreenZoneSize, minimumGreenZoneSize, difficultyProgress);

            // Speed increases as difficulty increases
            float qteSpeed = Mathf.Lerp(initialQTESpeed, maximumQTESpeed, difficultyProgress);

            quickTimeEvent.SetDifficulty(qteSpeed, greenZoneSize);
        }

        // Update enemy speed difficulty
        if (enemySpawner != null)
        {
            float enemySpeed = Mathf.Lerp(initialMainEnemySpeed, maximumMainEnemySpeed, difficultyProgress);
            enemySpawner.SetMainEnemySpeed(enemySpeed);
        }
    }

    public void RemoveLife()
    {
        if (_currentState != GameState.Playing) return;

        _lives--;
        OnLivesChanged?.Invoke(_lives);

        if (_lives <= 0)
        {
            TriggerGameOver();
        }
    }

    public void AddLife()
    {
        _lives++;
        OnLivesChanged?.Invoke(_lives);
    }

    private void TriggerGameOver()
    {
        _currentState = GameState.GameOver;

        if (_orderSpawnCoroutine != null)
        {
            StopCoroutine(_orderSpawnCoroutine);
            _orderSpawnCoroutine = null;
        }

        SetPlayerControlEnabled(false);
        OnGameOver?.Invoke();
    }

    public void PauseGame()
    {
        if (_currentState != GameState.Playing) return;

        _currentState = GameState.Paused;
        Time.timeScale = 0f;
        SetPlayerControlEnabled(false);
        OnGamePaused?.Invoke(true);
    }

    public void ResumeGame()
    {
        if (_currentState != GameState.Paused) return;

        _currentState = GameState.Playing;
        Time.timeScale = 1f;
        SetPlayerControlEnabled(true);
        OnGamePaused?.Invoke(false);
    }

    public void TogglePause()
    {
        if (_currentState == GameState.Playing)
            PauseGame();
        else if (_currentState == GameState.Paused)
            ResumeGame();
    }

    private void SetPlayerControlEnabled(bool enabled)
    {
        if (_playerMovement != null)
            _playerMovement.enabled = enabled;

        if (_playerInteractor != null)
            _playerInteractor.enabled = enabled;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        if (_orderSpawnCoroutine != null)
        {
            StopCoroutine(_orderSpawnCoroutine);
            _orderSpawnCoroutine = null;
        }

        _currentState = GameState.Idle;
        StartGame();
    }

    // Public getters for UI
    public int Score => _score;
    public int Lives => _lives;
    public int Complexity => _currentComplexity;
    public float CurrentOrderInterval => _currentOrderInterval;
}
