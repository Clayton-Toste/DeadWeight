using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteOrderer : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    void Start()
    {
        TryGetComponent(out spriteRenderer);
    }

    void Update()
    {
        spriteRenderer.sortingOrder = (int)(transform.position.y * -10000.0f);    
    }
}
