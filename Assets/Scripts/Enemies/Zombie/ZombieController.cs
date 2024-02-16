using Assets.Scripts.AI.Targeting;
using Assets.Scripts.DamageSystem;
using Assets.Scripts.Utils.Extensions;
using MarkusSecundus.PhysicsSwordfight.Utils.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField] Transform AttackCenter;
    [SerializeField] float AttackRadius = 1f;

    ITargetProvider _targetProvider;

    NavMeshAgent _navAgent;
    ZombieAnimationLogic _animationLogic;
    private void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _targetProvider = GetComponentInChildren<ITargetProvider>();
        _animationLogic = GetComponentInChildren<ZombieAnimationLogic>();
        _animationLogic.OnAttackSignal.AddListener(PerformAttack);
        if (!AttackCenter) AttackCenter = this.transform;
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

    void PerformAttack()
    {
        var attackedEntities = new HashSet<Damageable>();
        foreach(var collider in Physics.OverlapSphere(AttackCenter.transform.position, AttackRadius))
        {
            var armor = IArmorPiece.Get(collider);
            if (armor.IsNil() || false&&!attackedEntities.Add(armor.Damageable))
                continue;

            Debug.Log($"Attack: {collider.name} -> {armor} -> {armor.Damageable.name}");
        }
    }
}
