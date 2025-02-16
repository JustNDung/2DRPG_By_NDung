using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectPooling
{
    private Queue<GameObject> pool = new Queue<GameObject>();
    private GameObject prefab;
    private Transform parent;
    
    private float defaultLifeTime = 10f;

    public GameObjectPooling(GameObject prefab, int initialSize, Transform parent = null, float defaultLifeTime = 10f)
    {
        this.prefab = prefab;
        this.parent = parent;
        this.defaultLifeTime = defaultLifeTime;

        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    private GameObject CreateNewObject()
    {
        GameObject obj = Object.Instantiate(prefab, parent);
        obj.SetActive(false);
        pool.Enqueue(obj);
        return obj;
    }

    public GameObject Get(float? lifeTime = null)
    {
        if (pool.Count == 0)
        {
            CreateNewObject();
        }

        GameObject obj = pool.Dequeue();
        obj.SetActive(true);

        // Kiểm soát thời gian tồn tại
        float timeToLive = lifeTime ?? defaultLifeTime;
        if (timeToLive > 0)
        {
            CoroutineHelper.Instance.StartCoroutine(AutoReturn(obj, timeToLive));
        }

        return obj;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    private IEnumerator AutoReturn(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj.activeSelf) // Chỉ trả về nếu đối tượng vẫn đang hoạt động
        {
            Return(obj);
        }
    }
}

public class CoroutineHelper : MonoBehaviour
{
    private static CoroutineHelper _instance;
    public static CoroutineHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("CoroutineHelper");
                _instance = obj.AddComponent<CoroutineHelper>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }
}
