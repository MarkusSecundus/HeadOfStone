using MarkusSecundus.Utils.Datastructs;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.DamageSystem
{
    [System.Serializable]public enum DamageType
    {
        Piercing, Sharp, Blunt, Fire
    }

    [System.Serializable] public struct AttackDeclaration
    {
        public Object Attacker;
        public DamageType Type;
        public float Damage;
    }

    public interface IArmorPiece
    {
        private static readonly DefaultValWeakTable<GameObject, IArmorPiece> _armorPieceCache = new(k=> k.GetComponentInParent<IArmorPiece>());
        public static IArmorPiece Get(Collider c) => _armorPieceCache.GetOrDefault(c.gameObject);

        public void Attack(AttackDeclaration attackDeclaration);

        public Damageable Damageable { get; }
    }
}
