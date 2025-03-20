using Assets.Scripts.AI.Targeting;
using Assets.Scripts.DamageSystem;
using Assets.Scripts.DamageSystem.Damagers;
using MarkusSecundus.Utils.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class ZombieController : MonoBehaviour
{
    [SerializeField] float DistanceToAttack = 0f;
    [SerializeField] UnityEvent OnAttack;

    ITargetProvider _targetProvider;

    NavMeshAgent _navAgent;
    ZombieAnimationController _animationLogic;
    private void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _targetProvider = GetComponentInChildren<ITargetProvider>();
        _animationLogic = GetComponentInChildren<ZombieAnimationController>();
        _animationLogic.OnAttackSignal.AddListener(OnAttack.Invoke);
    }
    bool isDead = false;
    void Update()
    {
        if (!isDead && _targetProvider.Target)
        {
            _navAgent.isStopped = false;
            _navAgent.SetDestination(_targetProvider.Target.position);
        }
        else
            _navAgent.isStopped = true;
        float movementSpeed = _navAgent.velocity.magnitude;
        float remainingDistance = _navAgent.GetRemainingDistanceUntilStop();

        _animationLogic.UpdateAnimation((_targetProvider.Target && remainingDistance <= (DistanceToAttack + Mathf.Epsilon)), movementSpeed);
    }
    public void Die()
    {
        isDead = true;
        _animationLogic.Die();
    }
}
