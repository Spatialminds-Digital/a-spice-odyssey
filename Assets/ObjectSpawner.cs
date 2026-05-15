using Unity.Mathematics;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;

    public void SpawnObject(Transform position)
    {
        Instantiate(objectToSpawn, position.position, quaternion.identity);
    }

    
}
