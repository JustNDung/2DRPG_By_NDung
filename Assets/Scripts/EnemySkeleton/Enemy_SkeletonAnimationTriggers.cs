using UnityEngine;

public class Enemy_SkeletonAnimationTriggers : MonoBehaviour
{
    private Enemy_Skeleton enemySkeleton => GetComponentInParent<Enemy_Skeleton>();
    private void AnimationTrigger() {
        enemySkeleton.AnimationFinishTrigger();
    }
    private void AttackTrigger() {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(enemySkeleton.attackCheck.position, enemySkeleton.attackCheckRadius);
        foreach (var detectedObject in detectedObjects) {
            if (detectedObject.TryGetComponent(out Player player)) {
                player.Damage();
            }
        }
    }
    private void OpenCounterWindow() => enemySkeleton.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemySkeleton.CloseCounterAttackWindow();
}
