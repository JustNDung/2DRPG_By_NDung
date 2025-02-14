using UnityEngine;

public class Enemy_SkeletonAnimationTriggers : MonoBehaviour
{
    private Enemy_Skeleton enemySkeleton => GetComponentInParent<Enemy_Skeleton>();
    private void AnimationTrigger() {
        enemySkeleton.AnimationFinishTrigger();
    }
}
