using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemySpawner : MonoBehaviour
{
    private BoxCollider2D _bounds;

    [SerializeField] private Enemy mainEnemyPrefab;
    [SerializeField] private Enemy basicEnemyPrefab;
    [SerializeField] private OrderService orderService;
    [SerializeField] private Transform spaceShip;

    [Header("Basic Enemy Spawn Settings")]
    [SerializeField] private float minBasicSpawnInterval = 1f;
    [SerializeField] private float maxBasicSpawnInterval = 3f;
    [SerializeField] private float basicEnemySpeed = 3f;

    [Header("Main Enemy Settings")]
    [SerializeField] private float mainEnemyBaseSpeed = 2f;

    [Header("Pool Settings")]
    [SerializeField] private int defaultPoolSize = 10;
    [SerializeField] private int maxPoolSize = 50;

    private IObjectPool<Enemy> _mainEnemyPool;
    private IObjectPool<Enemy> _basicEnemyPool;
    private Dictionary<Order, Enemy> _orderEnemyMap = new Dictionary<Order, Enemy>();
    private List<Enemy> _activeBasicEnemies = new List<Enemy>();
    private Coroutine _basicSpawnCoroutine;

    private float _currentMainEnemySpeed;

    public Action<Enemy, Order> OnMainEnemyReachedPlayer;
    public Action<Enemy> OnEnemyKill;

    public List<Enemy> ActiveBasicEnemies => _activeBasicEnemies;


    void Awake()
    {
        _bounds = GetComponent<BoxCollider2D>();
        _currentMainEnemySpeed = mainEnemyBaseSpeed;

        _mainEnemyPool = new ObjectPool<Enemy>(
            createFunc: () => CreateEnemy(mainEnemyPrefab),
            actionOnGet: OnGetEnemy,
            actionOnRelease: OnReleaseEnemy,
            actionOnDestroy: OnDestroyEnemy,
            collectionCheck: false,
            defaultCapacity: defaultPoolSize,
            maxSize: maxPoolSize
        );

        _basicEnemyPool = new ObjectPool<Enemy>(
            createFunc: () => CreateEnemy(basicEnemyPrefab),
            actionOnGet: OnGetEnemy,
            actionOnRelease: OnReleaseEnemy,
            actionOnDestroy: OnDestroyEnemy,
            collectionCheck: false,
            defaultCapacity: defaultPoolSize,
            maxSize: maxPoolSize
        );
    }

    void OnEnable()
    {
        if (orderService != null)
        {
            orderService.OnNewOrderCreated += HandleNewOrder;
            orderService.OnOrderComplete += HandleOrderComplete;
            orderService.OnOrderRemoved += HandleOrderRemoved;
        }
    }

    void OnDisable()
    {
        if (orderService != null)
        {
            orderService.OnNewOrderCreated -= HandleNewOrder;
            orderService.OnOrderComplete -= HandleOrderComplete;
            orderService.OnOrderRemoved -= HandleOrderRemoved;
        }

        StopBasicEnemySpawning();
    }

    void Start()
    {
        StartBasicEnemySpawning();
    }

    #region Pool Methods
    private Enemy CreateEnemy(Enemy prefab)
    {
        Enemy enemy = Instantiate(prefab, transform);
        enemy.SetPool(enemy.IsMainEnemy ? _mainEnemyPool : _basicEnemyPool);
        return enemy;
    }

    private void OnGetEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private void OnReleaseEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        _activeBasicEnemies.Remove(enemy);
    }

    private void OnDestroyEnemy(Enemy enemy)
    {
        if (enemy != null)
            Destroy(enemy.gameObject);
    }
    #endregion

    #region Spawning
    public void StartBasicEnemySpawning()
    {
        if (_basicSpawnCoroutine == null)
            _basicSpawnCoroutine = StartCoroutine(BasicEnemySpawnRoutine());
    }

    public void StopBasicEnemySpawning()
    {
        if (_basicSpawnCoroutine != null)
        {
            StopCoroutine(_basicSpawnCoroutine);
            _basicSpawnCoroutine = null;
        }
    }

    private IEnumerator BasicEnemySpawnRoutine()
    {
        while (true)
        {
            float waitTime = UnityEngine.Random.Range(minBasicSpawnInterval, maxBasicSpawnInterval);
            yield return new WaitForSeconds(waitTime);
            SpawnBasicEnemy();
        }
    }

    private void SpawnBasicEnemy()
    {
        Enemy enemy = _basicEnemyPool.Get();
        enemy.transform.position = GetRandomSpawnPosition();
        _activeBasicEnemies.Add(enemy);

        var movement = enemy.GetComponent<EnemyMovement>();
        if (movement != null)
        {
            movement.SetEnemyMovement(basicEnemySpeed, spaceShip);
        }
    }

    private void SpawnMainEnemy(Order order)
    {
        Enemy enemy = _mainEnemyPool.Get();
        enemy.transform.position = GetRandomSpawnPosition();
        enemy.SetOrder(order);

        var movement = enemy.GetComponent<EnemyMovement>();
        if (movement != null)
        {
            movement.SetEnemyMovement(_currentMainEnemySpeed, spaceShip);
            movement.OnReachedPlayer += () => HandleMainEnemyReachedPlayer(enemy, order);
        }

        _orderEnemyMap[order] = enemy;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomY = UnityEngine.Random.Range(
            _bounds.bounds.min.y,
            _bounds.bounds.max.y
        );
        return new Vector3(_bounds.bounds.max.x, randomY, 0f);
    }
    #endregion

    #region Order Handling
    private void HandleNewOrder(Order order)
    {
        SpawnMainEnemy(order);
    }

    private void HandleOrderComplete(Order order)
    {
         if (_orderEnemyMap.TryGetValue(order, out Enemy enemy))
        {
            _orderEnemyMap.Remove(order);
            if (enemy != null)
            {
                enemy.Kill();
                OnEnemyKill?.Invoke(enemy);
            }
        }
    }

    private void HandleOrderRemoved(Order order)
    {
         if (_orderEnemyMap.TryGetValue(order, out Enemy enemy))
        {
            _orderEnemyMap.Remove(order);
            if (enemy != null)
            {
                enemy.Kill();
            }
        }
    }


    private void HandleMainEnemyReachedPlayer(Enemy enemy, Order order)
    {
        OnMainEnemyReachedPlayer?.Invoke(enemy, order);
    }
    #endregion

    #region Difficulty
    public void SetMainEnemySpeed(float speed)
    {
        _currentMainEnemySpeed = speed;
    }

    public float GetMainEnemySpeed() => _currentMainEnemySpeed;
    #endregion

    public Order GetOrderForEnemy(Enemy enemy)
    {
        foreach (var kvp in _orderEnemyMap)
        {
            if (kvp.Value == enemy)
                return kvp.Key;
        }
        return null;
    }

    public void RemoveOrderMapping(Order order)
    {
        _orderEnemyMap.Remove(order);
    }

    
}
