using Assets.Scripts.DamageSystem;
using Assets.Scripts.Utils.Extensions;
using MarkusSecundus.PhysicsSwordfight.Utils.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField] Transform TargetEnemy;
    [SerializeField] Transform AttackCenter;
    [SerializeField] float AttackRadius = 1f;
    

    NavMeshAgent navAgent;
    ZombieAnimationLogic animationLogic;
    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animationLogic = GetComponentInChildren<ZombieAnimationLogic>();
        animationLogic.OnAttackSignal.AddListener(PerformAttack);
        if (!AttackCenter) AttackCenter = this.transform;
    }
    void Update()
    {
        navAgent.SetDestination(TargetEnemy.position);
        float movementSpeed = navAgent.velocity.magnitude;
        float remainingDistance = navAgent.GetRemainingDistanceUntilStop();

        animationLogic.AnimationMovementSpeed = movementSpeed;
        animationLogic.AnimationAttackingState = remainingDistance <= Mathf.Epsilon;
    }

    void PerformAttack()
    {
        var attackedEntities = new HashSet<Damageable>();
        foreach(var collider in Physics.OverlapSphere(AttackCenter.transform.position, AttackRadius))
        {
            var armor = IArmorPiece.Get(collider);
            if (armor.IsNil() || false&&!attackedEntities.Add(armor.Damageable))
            {
                Debug.Log($"Cannot attack collider {collider.name}", this);
                continue;
            }
            Debug.Log($"Attack: {collider.name} -> {armor} -> {armor.Damageable.name}");
        }
    }
}
