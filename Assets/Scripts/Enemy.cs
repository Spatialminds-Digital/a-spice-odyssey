using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    [SerializeField] private bool isMainEnemy = false;
    [SerializeField] private Sprite[] enemyVisuals;
    [SerializeField] private SpriteRenderer enemyVisualRenderer;
    [SerializeField] private GameObject killParticle;

    [Header("Main Enemy")]
    [SerializeField] private SpriteRenderer _orderImage;

    public bool IsMainEnemy => isMainEnemy;

    private Order _enemyOrder;
    private IObjectPool<Enemy> _pool;

    public Order EnemyOrder => _enemyOrder;

    void OnEnable()
    {
        if (enemyVisuals.Length > 0)
            enemyVisualRenderer.sprite = enemyVisuals[Random.Range(0, enemyVisuals.Length)];
    }

    public void SetPool(IObjectPool<Enemy> pool)
    {
        _pool = pool;
    }

    public void SetOrder(Order order)
    {
        if (!isMainEnemy) return;
        _enemyOrder = order;
        if (_orderImage != null && _enemyOrder?.recipe?.sprite != null)
            _orderImage.sprite = _enemyOrder.recipe.sprite;
    }

    public void Kill()
    {
        //TODO: particle effects
        if(killParticle)
        {
            Instantiate(killParticle, transform.position, Quaternion.identity);
        }



        _enemyOrder = null;

        var movement = GetComponent<EnemyMovement>();
        if (movement != null)
            movement.ResetMovement();

        if (_pool != null)
            _pool.Release(this);
        else
            gameObject.SetActive(false);
    }
}
