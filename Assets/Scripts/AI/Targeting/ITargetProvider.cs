using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI.Targeting
{
    public interface ITargetProvider
    {
        public Transform Target { get; }
    }
}
