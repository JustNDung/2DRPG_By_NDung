using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public Dash_Skill dash {get; private set;}

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

    private void Start()
    {
        dash = GetComponent<Dash_Skill>();
    }
}
