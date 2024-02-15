using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.DamageSystem
{
    public class BasicArmorPiece : MonoBehaviour, IArmorPiece
    {
        public Damageable Damageable { get; private set; }

        [SerializeField] UnityEvent<AttackDeclaration> OnAttacked;

        void Start()
        {
            Damageable = Damageable.Get(this);
        }

        public void Attack(AttackDeclaration attackDeclaration)
        {
            Debug.Log($"Damage: {attackDeclaration.Attacker.name} attacks {Damageable.name} for {attackDeclaration.Damage} HP", this);
            OnAttacked?.Invoke(attackDeclaration);
            Damageable.Damage(attackDeclaration.Damage);
        }
    }
}
