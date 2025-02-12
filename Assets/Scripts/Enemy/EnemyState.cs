using UnityEngine;

public class EnemyState 
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemy; 

    protected bool triggerCalled;
    private string animBoolName;
    protected float stateTimer;
    
    public EnemyState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.enemy = enemy;
        this.animBoolName = animBoolName;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        enemy.anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        enemy.anim.SetBool(animBoolName, false);
    }
}
