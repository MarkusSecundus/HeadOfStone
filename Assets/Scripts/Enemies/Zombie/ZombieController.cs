using Assets.Scripts.AI.Targeting;
using Assets.Scripts.DamageSystem;
using Assets.Scripts.DamageSystem.Damagers;
using Assets.Scripts.Utils.Extensions;
using MarkusSecundus.PhysicsSwordfight.Utils.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField] IOnRequestDamager _damager;

    ITargetProvider _targetProvider;

    NavMeshAgent _navAgent;
    ZombieAnimationLogic _animationLogic;
    private void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _targetProvider = GetComponentInChildren<ITargetProvider>();
        _animationLogic = GetComponentInChildren<ZombieAnimationLogic>();
        _animationLogic.OnAttackSignal.AddListener(_damager.PerformAttack);
    }
    void Update()
    {
        if (_targetProvider.Target)
        {
            _navAgent.isStopped = false;
            _navAgent.SetDestination(_targetProvider.Target.position);
        }
        else
            _navAgent.isStopped = true;
        float movementSpeed = _navAgent.velocity.magnitude;
        float remainingDistance = _navAgent.GetRemainingDistanceUntilStop();

        _animationLogic.AnimationMovementSpeed = movementSpeed;
        _animationLogic.AnimationAttackingState = _targetProvider.Target && remainingDistance <= Mathf.Epsilon;
    }
}
