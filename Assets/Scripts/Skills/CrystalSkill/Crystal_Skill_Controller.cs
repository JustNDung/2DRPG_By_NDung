using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private float crystalExistTimer;
    
    public void SetupCrystal(float crystalDuration)
    {
        this.crystalExistTimer = crystalDuration;
    }
    
    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer <= 0)
        {
            SelfDestroy();
        }
    }
    
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
