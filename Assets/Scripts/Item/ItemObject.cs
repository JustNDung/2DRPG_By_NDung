using System;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private ItemData itemData;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = itemData.icon;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Debug.Log("Pick up item" + itemData.itemName);
            Destroy(gameObject);
        }
    }
}
