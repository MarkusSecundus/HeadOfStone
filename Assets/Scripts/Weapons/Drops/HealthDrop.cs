using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : AbstractDrop
{
    [SerializeField] float HP;

    protected override bool ApplyDrop(PlayerController player)
    {
        var damageable = player.GetComponentInChildren<Damageable>();
        if (!damageable) return false;

        return damageable.Heal(HP);
    }
}
