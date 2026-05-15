using System.Collections.Generic;
using UnityEngine;

public class SpaceshipMovement : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private float lerpSpeed = 5f;
    [SerializeField] private float yOffset = 0f;

    private List<Enemy> activeBasicEnemies;
    private float _smallestX;
    private Enemy _closest;
    Vector3 _pos;
    void Start()
    {
        activeBasicEnemies = enemySpawner.ActiveBasicEnemies;
    }

    void Update()
    {
        GetClosestBasicEnemy();

        if (_closest == null) return;

        _pos = transform.position;
       // _pos.y = _closest.transform.position.y;
        _pos.y = Mathf.Lerp(transform.position.y, _closest.transform.position.y + yOffset, lerpSpeed * Time.deltaTime);
        transform.position = _pos;

    }



    private void GetClosestBasicEnemy()
    {
        _closest = null;
        _smallestX = float.MaxValue;

        foreach (Enemy enemy in activeBasicEnemies)
        {
            if(enemy == null) continue;
            if(enemy.transform.position.x < transform.position.x) continue;

            if(enemy.transform.position.x < _smallestX)
            {
                _smallestX = enemy.transform.position.x;
                _closest = enemy;
            }
            
        }

    }
}
