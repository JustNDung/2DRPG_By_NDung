using UnityEngine;

public class Sword_Skill : Skill
{
    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchDirection;
    [SerializeField] private float swordGravity;
    [SerializeField] private int swordAmount;
    private GameObjectPooling swordPool;

    protected override void Start()
    {
        base.Start();
        swordPool = new GameObjectPooling(swordPrefab, swordAmount);
    }
    public void CreateSword()
    {
        GameObject newSword = swordPool.Get();
        newSword.transform.position = player.transform.position;
        newSword.GetComponent<Sword_Skill_Controller>().SetupSword(launchDirection, swordGravity);
    }
}
