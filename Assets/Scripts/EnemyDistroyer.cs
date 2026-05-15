using UnityEngine;

public class EnemyDistroyer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("BasicEnemy"))
        {
            collision.gameObject.GetComponent<Enemy>().Kill();
        }
    }
}
