using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.AI.Targeting
{

    public sealed class FactionMember : MonoBehaviour
    {
        public static FactionMember Get(Component self) => self.GetComponentInParent<FactionMember>();

        [field: SerializeField] public FactionName Faction { get; private set; } = FactionName.Hostile;
    }
}
