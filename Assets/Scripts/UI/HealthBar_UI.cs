using System;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar_UI : MonoBehaviour
{
    private Entity entity;
    private CharacterStats characterStats;
    private RectTransform rectTransform;
    private Slider slider;

    private void Start()
    {
        characterStats = GetComponentInParent<CharacterStats>();
        slider = GetComponentInChildren<Slider>();
        rectTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        
        entity.onFlipped += FlipUI;
        characterStats.onHealthChanged += UpdateHealthUI;
        UpdateHealthUI();
    }
    
    private void UpdateHealthUI()
    {
        slider.maxValue = characterStats.GetMaxHp();
        slider.value = characterStats.currentHp;
    }

    private void FlipUI()
    {
        Debug.Log("Flipping UI");
        rectTransform.Rotate(0, 180, 0);
    }

    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        characterStats.onHealthChanged -= UpdateHealthUI;
    }
}
