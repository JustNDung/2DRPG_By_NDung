using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
