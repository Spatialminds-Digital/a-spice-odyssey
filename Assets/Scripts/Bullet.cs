using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;

    private IObjectPool<Bullet> pool;

    public void SetPool(IObjectPool<Bullet> bulletPool)
    {
        pool = bulletPool;
    }

    void Start()
    {
        Invoke(nameof(ReturnToPool), 5);
    }

    void Update()
    {
        transform.Translate(transform.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ReturnToPool();
        if(other.CompareTag("BasicEnemy"))
        {
            other.GetComponent<Enemy>().Kill();

            AudioService.Instance.PlayMiniExplosion();
            EffectService.Instance.camShake.Shake(1);
        }
    }

    private void ReturnToPool()
    {
        if (pool != null)
        {
            pool.Release(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
