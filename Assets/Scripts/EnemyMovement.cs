using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float xDistanceForAttack = 5f;
    [SerializeField] private float reachDistance = 0.5f;

    private float _speed;
    private Transform _spaceShip;
    private bool _isAttacking = false;
    private bool _hasReachedPlayer = false;

    private Enemy _enemy;

    public Action OnReachedPlayer;

    void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        if (_speed <= 0 || _hasReachedPlayer) return;

        if (_enemy.IsMainEnemy)
        {
            if (!_isAttacking && _spaceShip != null)
            {
                float xDistance = transform.position.x - _spaceShip.position.x;
                if (xDistance <= xDistanceForAttack)
                {
                    _isAttacking = true;
                }
            }

            if (_isAttacking && _spaceShip != null)
            {
                Vector3 direction = (_spaceShip.position - transform.position).normalized;
                transform.position += direction * _speed * Time.deltaTime;

                float distance = Vector3.Distance(transform.position, _spaceShip.position);
                if (distance <= reachDistance)
                {
                    _hasReachedPlayer = true;
                    OnReachedPlayer?.Invoke();
                }
            }
            else
            {
                transform.position += Vector3.left * _speed * Time.deltaTime;
            }
        }
        else
        {
            transform.position += Vector3.left * _speed * Time.deltaTime;
        }
    }

    public void SetEnemyMovement(float speed, Transform spaceShip)
    {
        _speed = speed;
        _spaceShip = spaceShip;
        _isAttacking = false;
        _hasReachedPlayer = false;
        OnReachedPlayer = null;
    }

    public void ResetMovement()
    {
        _speed = 0;
        _spaceShip = null;
        _isAttacking = false;
        _hasReachedPlayer = false;
        OnReachedPlayer = null;
    }
}
