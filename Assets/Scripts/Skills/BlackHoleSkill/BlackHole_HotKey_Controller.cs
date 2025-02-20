using TMPro;
using UnityEngine;

public class BlackHole_HotKey_Controller : MonoBehaviour
{
    private SpriteRenderer mySpriteRenderer;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform enemy;
    private BlackHole_Skill_Controller blackHoleSkillController;

    public void SetupHotKey(KeyCode myHotKey, Transform enemy, BlackHole_Skill_Controller blackHoleSkillController)
    {
        myText = GetComponentInChildren<TextMeshProUGUI>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        this.enemy = enemy;
        this.blackHoleSkillController = blackHoleSkillController;
        this.myHotKey = myHotKey;
        myText.text = this.myHotKey.ToString();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            blackHoleSkillController.AddEnemyToList(this.enemy);

            myText.color = Color.clear;
            mySpriteRenderer.color = Color.clear;
        }
    }
}
