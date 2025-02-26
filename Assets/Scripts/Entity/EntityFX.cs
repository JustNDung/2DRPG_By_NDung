using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Flash FX")] 
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMaterial;
    private Material defaultMaterial;

    [Header("Ailment colors")] 
    [SerializeField] private Color[] chillColors;
    [SerializeField] private Color[] igniteColors;
    [SerializeField] private Color[] shockColors;
    [SerializeField] private float igniteRepeatRate = 0.3f;
    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); 
        defaultMaterial = spriteRenderer.material;   
    }
    
    public void MakeTransparent(bool transparent)
    {
        if (transparent)
        {
            spriteRenderer.color = Color.clear;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    private IEnumerator FlashFX()
    {
        spriteRenderer.material = hitMaterial;
        Color currentColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = currentColor;
        spriteRenderer.material = defaultMaterial;
    }

    private void RedColorBlink() {
        if (spriteRenderer.color != Color.white) {
            spriteRenderer.color = Color.white;
        } else {
            spriteRenderer.color = Color.red;
        }
    }

    private void CancelColorChange() {
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }

    public void IgniteFxFor(float seconds)
    {
        InvokeRepeating("IgniteColorFx", 0, igniteRepeatRate);
        Invoke("CancelColorChange", seconds);
    }
    
    public void ChillFxFor(float seconds)
    {
        InvokeRepeating("ChillColorFx", 0, igniteRepeatRate);
        Invoke("CancelColorChange", seconds);
    }
    
    public void ShockFxFor(float seconds)
    {
        InvokeRepeating("ShockColorFx", 0, igniteRepeatRate);
        Invoke("CancelColorChange", seconds);
    }

    private void IgniteColorFx()
    {
        if (spriteRenderer.color != igniteColors[0])
        {
            spriteRenderer.color = igniteColors[0];
        }
        else
        {
            spriteRenderer.color = igniteColors[1];
        }
    }
    
    private void ChillColorFx()
    {
        if (spriteRenderer.color != chillColors[0])
        {
            spriteRenderer.color = chillColors[0];
        }
        else
        {
            spriteRenderer.color = chillColors[1];
        }
    }

    private void ShockColorFx()
    {
        if (spriteRenderer.color != shockColors[0])
        {
            spriteRenderer.color = shockColors[0];
        }
        else
        {
            spriteRenderer.color = shockColors[1];
        }
    }
}
