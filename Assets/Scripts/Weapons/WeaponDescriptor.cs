using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponDescriptor : MonoBehaviour
{
    public KeyCode ActivationKey;
    public string GunName;
    public int MaxAmmo;
    public bool IsEnabled = false;

    public int CurrentAmmo = -1;
    public bool HasAmmo => CurrentAmmo != 0;
    public bool HasInfiniteAmmo => MaxAmmo < 0;

    public UnityEvent<WeaponDescriptor> OnStateUpdated;

    public struct AmmoUpdateEventArgs
    {
        public int OriginalAmmo { get; init; }
    }

    public bool AddAmmo(int amount)
    {
        if (HasInfiniteAmmo) return true;

        var oldAmmo = CurrentAmmo;
        var newAmmo = CurrentAmmo = Mathf.Clamp(oldAmmo + amount, 0, MaxAmmo);
        OnStateUpdated?.Invoke(this);
        return oldAmmo != newAmmo;
    }

    private void Awake()
    {
        if (CurrentAmmo < 0) CurrentAmmo = MaxAmmo;
    }
}
