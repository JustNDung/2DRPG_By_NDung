
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlackHole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> hotKeys;
    
    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float blackHoleTimer;
    
    private bool canGrow = true;
    private bool canShrink;
    private bool playerCanDisappear = true;

    private bool cloneAttackRelease;
    private bool canCreateHotkey = true;
    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> hotkeys = new List<GameObject>();

    [SerializeField] private float finishBlackHoleAbilityDelay = 0.5f;
    public bool playerCanExitState { get; private set; }
    
    public void SetupBlackHole(float maxSize, float growSpeed, float shrinkSpeed
        , int amountOfAttacks, float cloneAttackCooldown, float blackHoleDuration)
    {
        this.maxSize = maxSize;
        this.growSpeed = growSpeed;
        this.shrinkSpeed = shrinkSpeed;
        this.amountOfAttacks = amountOfAttacks;
        this.cloneAttackCooldown = cloneAttackCooldown;
        blackHoleTimer = blackHoleDuration;
    }
    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackHoleTimer -= Time.deltaTime;
        
        if (blackHoleTimer < 0)
        {
            blackHoleTimer = Mathf.Infinity;

            if (targets.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
            {
                FinishBlackHoleAbility();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }
        
        CloneAttackLogic();
        
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale
                , new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime
            );
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale
                , new Vector2(-1, -1), shrinkSpeed * Time.deltaTime
            );
            if (transform.localScale.x < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0)
        {
            return;
        }
        DestroyHotKeys();
        cloneAttackRelease = true;
        canCreateHotkey = false;

        if (playerCanDisappear)
        {
            playerCanDisappear = false;
            PlayerManager.instance.player.MakeTransparent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackRelease && amountOfAttacks > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;
            int randomIndex = Random.Range(0, targets.Count);
            
            float xOffset;
            if (Random.Range(0, 100) > 50)
            {
                xOffset = 2f;
            }
            else
            {
                xOffset = -2f;
            }
            
            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0, 0));
            amountOfAttacks--;
            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackHoleAbility", finishBlackHoleAbilityDelay);
            }
        }
    }

    private void FinishBlackHoleAbility()
    {
        DestroyHotKeys();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackRelease = false;
    }

    private void DestroyHotKeys()
    {
        if (hotkeys.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < hotkeys.Count; i++)
        {
            Destroy(hotkeys[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.FreezeTime(true);
            CreateHotKey(other);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.FreezeTime(false);
        }
    }

    private void CreateHotKey(Collider2D other)
    {
        if (!canCreateHotkey)
        {
            return;
        }
        if (hotKeys.Count <= 0)
        {
            Debug.Log("No more hotkeys");
            return;
        }
        GameObject newHotKey = Instantiate(hotKeyPrefab, other.transform.position + new Vector3(0, 2), Quaternion.identity);
        hotkeys.Add(newHotKey);
        
        KeyCode chosenKey = hotKeys[Random.Range(0, hotKeys.Count)];
        hotKeys.Remove(chosenKey);
        
        BlackHole_HotKey_Controller blackHoleHotKeyController = newHotKey.GetComponent<BlackHole_HotKey_Controller>();
        blackHoleHotKeyController.SetupHotKey(chosenKey, other.transform, this);
    }
    
    public void AddEnemyToList(Transform enemy) => targets.Add(enemy);
}
