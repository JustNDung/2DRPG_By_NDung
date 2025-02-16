using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float colorLosingSpeed;
    private float cloneTimer;
    private Color originalColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a - colorLosingSpeed * Time.deltaTime);
        }
        
    }
    public void SetupClone(Transform newTransform, float cloneDuration)
    {
        spriteRenderer.color = originalColor;
        transform.position = newTransform.position;
        cloneTimer = cloneDuration;
    }
}
