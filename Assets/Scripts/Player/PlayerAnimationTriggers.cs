using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private void AnimationTrigger() {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] detectedObjects =
            Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var detectedObject in detectedObjects)
        {
            if (detectedObject.TryGetComponent(out Enemy enemy))
            {
                enemy.Damage();
            }
        }
    }
}
