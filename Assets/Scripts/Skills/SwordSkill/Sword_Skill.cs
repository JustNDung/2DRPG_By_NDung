using UnityEngine;
using UnityEngine.Serialization;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}
public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;
    
    [Header("Bounce info")] 
    [SerializeField] private int amountOfBounce;
    [SerializeField] private float bounceGravity;
    
    [Header("Pierce info")]
    [SerializeField] private int amountOfPierce;
    [SerializeField] private float pierceGravity;
    
    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [FormerlySerializedAs("launchDirection")] [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    private Vector2 finalDir;

    [Header("Aim dots")] 
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;
    protected override void Start()
    {
        base.Start();
        GenerateDots();
        SetupGravity();
    }

    private void SetupGravity()
    {
        switch (swordType)
        {
            case SwordType.Regular:
                swordGravity = 1.0f; // Default gravity for Regular type
                break;
            case SwordType.Bounce:
                swordGravity = bounceGravity;
                break;
            case SwordType.Pierce:
                swordGravity = pierceGravity;
                break;
            case SwordType.Spin:
                swordGravity = 0.5f; // Example gravity for Spin type
                break;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x,
                AimDirection().normalized.y * launchForce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }
    
    // Tạo sword rồi set lực và vị trí cho nó.
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, Quaternion.identity);
        Sword_Skill_Controller newSwordController = newSword.GetComponent<Sword_Skill_Controller>();

        
        switch (swordType)
        {
            case SwordType.Bounce:
                newSwordController.SetupBounce(true, amountOfBounce);
                break;
            case SwordType.Pierce:
                newSwordController.SetupPierce(amountOfPierce);
                break;
            // Add other cases if needed
        }
        newSwordController.SetupSword(finalDir, swordGravity, player);
        player.AssignNewSword(newSword);
        DotsActive(false);
    }

    #region Aim region
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction;
    }

    public void DotsActive(bool isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(isActive);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
                                                                  AimDirection().normalized.x * launchForce.x,
                                                                  AimDirection().normalized.y * launchForce.y) * t
                                                              + 0.5f * (Physics2D.gravity * swordGravity) * t * t;
        // swordGravity để điều chỉnh hệ thống trọng lực trong Unity giúp thêm những hiệu ứng vật lý cho thanh kiếm.
        // Công thức vât lý : p + vt + 1/2 * a * t^2
        return position;
    }
    #endregion
}
