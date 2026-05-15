using UnityEngine;

public class SpaceshipCannon : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private GameObject cannonObject;
    [SerializeField] private float timeToStayAlive;

    [SerializeField] private Transform originPoint;

    [SerializeField] private EnemySpawner spawner;
    private float _timer;

    void Start()
    {
        cannonObject.SetActive(false);
    }

    void Update()
    {
        if(_timer>0)
            _timer -= Time.deltaTime;

        if(_timer<0)
        {
            cannonObject.SetActive(false);
        }

        line.SetPosition(0, originPoint.position);

    }

    void OnEnable()
    {
        if(!spawner) return;
        
        spawner.OnEnemyKill += FireCannon;
    }

    void OnDisable()
    {
        
        if(!spawner) return;
        
        spawner.OnEnemyKill -= FireCannon;
    }

    private void FireCannon(Enemy enemy)
    {
        cannonObject.SetActive(true);

        line.SetPosition(1, enemy.transform.position);
        _timer = timeToStayAlive;

        //Play sound etc.
    }

}
