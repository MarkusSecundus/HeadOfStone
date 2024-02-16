using MarkusSecundus.PhysicsSwordfight.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI.Targeting
{
    public class AreaTargetProvider : MonoBehaviour, ITargetProvider
    {
        public Transform Target => _target?_target.transform:null;
        FactionMember _target;

        [SerializeField] bool CanAttackOwnFaction = false;

        FactionMember ownFaction;

        void Start()
        {
            ownFaction = FactionMember.Get(this);
        }

        HashSet<FactionMember> activeTargets = new();

        private void OnTriggerEnter(Collider other)
        {
            var target = FactionMember.Get(other);
            if (!target || ReferenceEquals(target, ownFaction) || (!CanAttackOwnFaction && target.Faction == ownFaction.Faction))
                return;
            activeTargets.Add(target);
            if (!_target)
                _target = target;
        }
        private void OnTriggerExit(Collider other)
        {
            var target = FactionMember.Get(other);
            if (!target || !activeTargets.Contains(target))
                return;

            activeTargets.Remove(target);
            if (_target == target || !_target.gameObject.activeInHierarchy)
            {
                _target = null;
                foreach (var candidate in activeTargets)
                {
                    if (!candidate.gameObject.activeInHierarchy) continue;
                    _target = candidate;
                    break;
                }
            }
        }
    }
}
