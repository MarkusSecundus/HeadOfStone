using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponDrop : AbstractDrop
{
    [SerializeField] WeaponType _weapon;
    [SerializeField] int _ammoCount;


    protected override bool ApplyDrop(PlayerController player)
    {
        var weapon = player.GetComponentsInChildren<WeaponDescriptor>(true).First(w => w.WeaponType == _weapon);
        if (!weapon) 
            return false;

        if (!weapon.IsEnabled) //activate a new weapon
        {
            weapon.IsEnabled = true;
            weapon.AddAmmo(_ammoCount);
            player.GetComponentInChildren<WeaponListManager>().SwitchWeapon(weapon);
            return true;
        }

        // weapon is already active, just add ammo to it if possible
        return weapon.AddAmmo(_ammoCount);
    }
}
