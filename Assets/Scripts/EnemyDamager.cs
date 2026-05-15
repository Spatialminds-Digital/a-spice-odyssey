using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private OrderService orderService;
    [SerializeField] private GameplayManager gameplayManager;

    void OnEnable()
    {
        if (enemySpawner != null)
        {
            enemySpawner.OnMainEnemyReachedPlayer += HandleEnemyReachedPlayer;
        }
    }

    void OnDisable()
    {
        if (enemySpawner != null)
        {
            enemySpawner.OnMainEnemyReachedPlayer -= HandleEnemyReachedPlayer;
        }
    }

    private void HandleEnemyReachedPlayer(Enemy enemy, Order order)
    {
        if (enemy == null) return;

        // Remove the order from the service
        if (orderService != null && order != null)
        {
            orderService.RemoveOrder(order);
        }

        // Remove from spawner tracking
        if (enemySpawner != null && order != null)
        {
            enemySpawner.RemoveOrderMapping(order);
        }

        // Kill the enemy
        enemy.Kill();

        // Remove a life from the player
        if (gameplayManager != null)
        {
            gameplayManager.RemoveLife();
        }
    }
}
