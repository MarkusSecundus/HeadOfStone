using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZombieAnimationLogic : MonoBehaviour
{
    Animator animator;

    public UnityEvent OnAttackSignal;

    static class AnimatorVariables
    {
        public const string MovementSpeed = nameof(MovementSpeed);
        public const string Attacking = nameof(Attacking);
    }
    public float AnimationMovementSpeed { set => animator.SetFloat(AnimatorVariables.MovementSpeed, value); }
    public bool AnimationAttackingState { set => animator.SetBool(AnimatorVariables.Attacking, value); }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void PerformAttack_Signal()
    {
        Debug.Log($"{name} - Performing attack!", this);
        OnAttackSignal?.Invoke();
    }
}
