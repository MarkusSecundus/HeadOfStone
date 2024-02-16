using MarkusSecundus.PhysicsSwordfight.PhysicsUtils;
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
            activeTargets = new(FactionMember.Get, target => !ReferenceEquals(target, ownFaction) && (CanAttackOwnFaction || target.Faction != ownFaction.Faction));
        }

        ColliderActivityInfo<FactionMember> activeTargets;

        private void OnTriggerEnter(Collider other)
        {
            var target = activeTargets.Enter(other);
            if (target && !_target)
                _target = target;
        }
        private void OnTriggerExit(Collider other)
        {
            var target = activeTargets.Exit(other);
            if (!target) return;

            if (!_target || _target == target || !_target.gameObject.activeInHierarchy)
            {
                _target = null;
                foreach (var candidate in activeTargets.Active)
                {
                    if (!candidate.gameObject.activeInHierarchy) continue;
                    _target = candidate;
                    break;
                }
            }
        }
    }
}
