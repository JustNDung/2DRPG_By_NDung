using System;
using System.Collections.Generic;
using UnityEngine;
public class BlackHole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> hotKeys;
    
    public float maxSize;
    public float growSpeed;
    public bool canGrow;

    private List<Transform> targets = new List<Transform>();
    private void Update()
    {
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale
                , new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime
            );
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

    private void CreateHotKey(Collider2D other)
    {
        if (hotKeys.Count <= 0)
        {
            Debug.Log("No more hotkeys");
            return;
        }
        GameObject newHotKey = Instantiate(hotKeyPrefab, other.transform.position + new Vector3(0, 2), Quaternion.identity);
        KeyCode chosenKey = hotKeys[UnityEngine.Random.Range(0, hotKeys.Count)];
        hotKeys.Remove(chosenKey);
        BlackHole_HotKey_Controller blackHoleHotKeyController = newHotKey.GetComponent<BlackHole_HotKey_Controller>();
        blackHoleHotKeyController.SetupHotKey(chosenKey, other.transform, this);
    }
    
    public void AddEnemyToList(Transform enemy) => targets.Add(enemy);
}
