﻿using MarkusSecundus.PhysicsSwordfight.PhysicsUtils;
using MarkusSecundus.PhysicsSwordfight.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.DamageSystem.Damagers
{
    public class AreaDamagerBase : IOnRequestDamager
    {
        [SerializeField] float Damage;
        [SerializeField] DamageType DamageType;
        [SerializeField] UnityEvent<AttackDeclaration, IArmorPiece> OnAttacked;

        ColliderActivityInfo<IArmorPiece> _activeTargets;
        Damageable _thisDamageable;
        private void Start()
        {
            _thisDamageable = Damageable.Get(this);
            _activeTargets = new(IArmorPiece.Get, a => a.Damageable != _thisDamageable);
        }

        public override void PerformAttack()
        {
            var declaration = new AttackDeclaration { Attacker = this, Damage = Damage, Type = DamageType };
            foreach(var target in _activeTargets.Active)
            {
                if(target.Damageable.gameObject.activeInHierarchy)
                    target.Attack(declaration);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            _activeTargets.Enter(other);
        }
        private void OnTriggerExit(Collider other)
        {
            _activeTargets.Exit(other);
        }
    }
}