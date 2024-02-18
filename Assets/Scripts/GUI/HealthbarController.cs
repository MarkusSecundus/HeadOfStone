using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour
{
    [SerializeField] Damageable TargetDamageable;

    Scrollbar _scrollbar;

    private void Awake()
    {
        _scrollbar = GetComponentInChildren<Scrollbar>();
    }

    private void OnEnable()
    {
        TargetDamageable.OnUpdate.AddListener(UpdateState);
    }
    private void OnDisable()
    {
        TargetDamageable.OnUpdate.RemoveListener(UpdateState);
    }

    private void UpdateState(Damageable.HealthChangeInfo info)
    {
        _scrollbar.size = info.ResultHP / ((float)info.Damageable.MaxHP);
    }
}
