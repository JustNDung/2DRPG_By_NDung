using UnityEngine;

public class PlayerState
{
    protected Rigidbody2D rb;
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected float xInput;
    private string animBoolName; 
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;  
    }
    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
    }
    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
    }
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false); 
    }
}
