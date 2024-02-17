using Assets.Scripts.AI.Targeting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class ZombieAnimationController : MonoBehaviour
{
    public enum AnimationState
    {
        None=0, Idle, Walk, Attack, Die
    }

    Animator animator;

    public UnityEvent OnAttackSignal;
    public UnityEvent OnDeathSignal;

    static class AnimatorVariables
    {
        public const string MovementSpeed = nameof(MovementSpeed);
        public const string Attacking = nameof(Attacking);
        public const string DeathType = nameof(DeathType);
        public const int DeathTypesCount = 2;
    }

    public void Die()
    {
        animator.SetInteger(AnimatorVariables.DeathType, Random.Range(1, AnimatorVariables.DeathTypesCount));
        State = AnimationState.Die; 
    }

    AnimationState _state = AnimationState.None;
    public AnimationState State { get => _state; private set 
        {
            //Debug.Log($"Transition {value} -> {_state}", this);
            if(_state != AnimationState.None)
                animator.SetBool(_state.ToString(), false);
            if((_state = value) != AnimationState.None)
                animator.SetBool(_state.ToString(), true);
        } }
    public void UpdateAnimation(bool attacking, float movementSpeed)
    {
        if (State == AnimationState.Die) return;
        if (attacking)
            State = ZombieAnimationController.AnimationState.Attack;
        else
            State = movementSpeed > 0 ? ZombieAnimationController.AnimationState.Walk : ZombieAnimationController.AnimationState.Idle;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        State = AnimationState.Idle;
    }

    void PerformAttack_Signal()
    {
        OnAttackSignal?.Invoke();
    }

    void PerformDeath_Signal()
    {
        OnDeathSignal?.Invoke();
    }
}
