using UnityEngine;

public class PlayerState : MonoBehaviour
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    private string animBoolName; 
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;  
    }
    public virtual void Enter()
    {
        Debug.Log("Entered " + this.GetType().Name);
    }
    public virtual void Update()
    {
        Debug.Log("Exited " + this.GetType().Name);
    }
    public virtual void Exit()
    {
        Debug.Log("Updated " + this.GetType().Name);
    }
}
