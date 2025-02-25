using System;
using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private float speed;
    [SerializeField] private string hit = "Hit";
    [SerializeField] private float thunderDamage = 1f;
    [SerializeField] private float destroyDelay = .4f;
    [SerializeField] private float damageDelay = .2f;
    [SerializeField] private Vector3 maxScale = new Vector3(3, 3);
    
    private Animator animator;
    private bool triggered;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (triggered)
        {
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, characterStats.transform.position
            , speed * Time.deltaTime
        );
        transform.right = transform.position - characterStats.transform.position;
        if (Vector2.Distance(transform.position, characterStats.transform.position) < .1f)
        {
            animator.transform.localPosition = new Vector3(0, 0.5f);
            animator.transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale = maxScale; 
            
            Invoke("DamageAndSelfDestroy", damageDelay);
            triggered = true;
            animator.SetTrigger(hit);
        }
        
    }

    private void DamageAndSelfDestroy()
    {
        characterStats.TakeDamage(thunderDamage);
        Destroy(gameObject, destroyDelay); 
    }
}
