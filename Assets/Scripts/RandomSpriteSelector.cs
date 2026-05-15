using UnityEngine;

public class RandomSpriteSelector : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spRenderer;
    [SerializeField] private Sprite[] sprites;

    void Start()
    {
        spRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
