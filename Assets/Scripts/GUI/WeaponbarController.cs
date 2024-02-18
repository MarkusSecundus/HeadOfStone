using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponbarController : MonoBehaviour
{
    [SerializeField] WeaponListManager TargetWeaponList;
    [SerializeField] string Format = "{0} ({1} / {2})";
    [SerializeField] string FormatWhenInfiniteAmmo = "{0}";

    TMP_Text _text;

    private void Awake()
    {
        _text = GetComponentInChildren<TMP_Text>();
    }

    private void OnEnable()
    {
        TargetWeaponList.OnStateUpdated.AddListener(UpdateState);
    }
    private void OnDisable()
    {
        TargetWeaponList.OnStateUpdated.RemoveListener(UpdateState);
    }

    private void UpdateState(WeaponDescriptor info)
    {
        _text.text = string.Format(info.HasInfiniteAmmo?FormatWhenInfiniteAmmo:Format, info.GunName, info.CurrentAmmo, info.MaxAmmo);
    }
}
