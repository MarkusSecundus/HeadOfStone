﻿using MarkusSecundus.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.DamageSystem
{
    public class ImpactDamager : MonoBehaviour
    {
        [SerializeField] float Damage;
        [SerializeField] DamageType DamageType;
        [SerializeField] UnityEvent<AttackDeclaration, IArmorPiece> OnAttacked; 

        void OnCollisionEnter(Collision collision) => DoDamage(IArmorPiece.Get(collision.collider));
        void OnTriggerEnter(Collider other) => DoDamage(IArmorPiece.Get(other));

        void DoDamage(IArmorPiece armorPiece)
        {
            if (armorPiece.IsNil()) return;

            var attackDeclaration = new AttackDeclaration { Attacker = this, Damage = Damage, Type = DamageType };
            OnAttacked?.Invoke(attackDeclaration, armorPiece);
            armorPiece.Attack(attackDeclaration);
        }
    }
}
