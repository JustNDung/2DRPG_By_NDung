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

    private void RedColorBlink() {
        if (spriteRenderer.color != Color.white) {
            spriteRenderer.color = Color.white;
        } else {
            spriteRenderer.color = Color.red;
        }
    }

    private void CancelRedBlink() {
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }
}
