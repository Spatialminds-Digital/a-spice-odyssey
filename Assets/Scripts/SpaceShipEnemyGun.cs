using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class SpaceShipEnemyGun : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float fireInterval = 1f;
    [SerializeField] private int poolDefaultCapacity = 10;
    [SerializeField] private int poolMaxSize = 20;

    public UnityEvent OnFire;

    private IObjectPool<Bullet> bulletPool;
    private float fireTimer;

    void Awake()
    {
        bulletPool = new ObjectPool<Bullet>(
            createFunc: CreateBullet,
            actionOnGet: OnGetBullet,
            actionOnRelease: OnReleaseBullet,
            actionOnDestroy: OnDestroyBullet,
            collectionCheck: true,
            defaultCapacity: poolDefaultCapacity,
            maxSize: poolMaxSize
        );
    }

    void Update()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireInterval)
        {
            FireBullet();
            fireTimer = 0f;
        }
    }

    private void FireBullet()
    {
        bulletPool.Get();
        AudioService.Instance.PlayBulletSound();

        OnFire?.Invoke();

    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        bullet.SetPool(bulletPool);

        

        return bullet;
    }

    private void OnGetBullet(Bullet bullet)
    {
        bullet.transform.position = spawnPoint.position;
        bullet.transform.rotation = spawnPoint.rotation;
        bullet.gameObject.SetActive(true);
    }

    private void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}
