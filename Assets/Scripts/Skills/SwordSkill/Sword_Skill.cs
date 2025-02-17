using UnityEngine;
using UnityEngine.Serialization;

public class Sword_Skill : Skill
{
    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [FormerlySerializedAs("launchDirection")] [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private int swordAmount;
    private GameObjectPooling swordPool;
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
        swordPool = new GameObjectPooling(swordPrefab, swordAmount);
        GenerateDots();
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
        GameObject newSword = swordPool.Get();
        newSword.transform.position = player.transform.position;
        newSword.GetComponent<Sword_Skill_Controller>().SetupSword(finalDir, swordGravity);
        DotsActive(false);
    }

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
}
