using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public Dash_Skill dash {get; private set;}
    public Clone_Skill clone {get; private set;}
    public Sword_Skill sword { get; private set; }
    public BlackHole_Skill blackHole { get; private set; }
    public Crystal_Skill crystal { get; private set; }
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
        clone = GetComponent<Clone_Skill>();
        sword = GetComponent<Sword_Skill>();
        blackHole = GetComponent<BlackHole_Skill>();
        crystal = GetComponent<Crystal_Skill>();
    }
}
