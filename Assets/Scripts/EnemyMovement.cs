using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private bool isMainEnemy = false;
    [SerializeField] private float xDistanceForAttack = 5f;
    private float _speed;
    private Transform _spaceShip;
    private bool _isAttacking = false;

    public bool IsMainEnemy => isMainEnemy;

    void Update()
    {
        if (_speed <= 0) return;

        if (isMainEnemy)
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
    }
}
