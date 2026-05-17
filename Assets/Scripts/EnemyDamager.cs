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

        // Life lose Audio / Effect
        EffectService.Instance.ShowRedVignette(1);
        AudioService.Instance.PlayBigExplosion();
        EffectService.Instance.camShake.Shake(10);

        EffectService.Instance.ShowMessage("LIFE LOST!", Color.softRed, 2);


        // Remove a life from the player
        if (gameplayManager != null)
        {
            gameplayManager.RemoveLife();
        }
    }
}
