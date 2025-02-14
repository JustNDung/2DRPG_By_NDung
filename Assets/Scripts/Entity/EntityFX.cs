using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Flash FX")] 
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMaterial;
    private Material defaultMaterial;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); 
        defaultMaterial = spriteRenderer.material;   
    }

    private IEnumerator FlashFX()
    {
        spriteRenderer.material = hitMaterial;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.material = defaultMaterial;
    }
}
