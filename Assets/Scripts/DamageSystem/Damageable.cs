using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class Damageable : MonoBehaviour
{
    public static Damageable Get(Object o) => o.GetComponentInParent<Damageable>();

    [field: SerializeField] public float MaxHP { get; private set; } = 100;
    public const float MinHP = 0;

    [field: SerializeField]public float HP { get; private set; }

    [SerializeField] UnityEvent<HealthChangeInfo> OnDamaged;
    [SerializeField] UnityEvent<HealthChangeInfo> OnHealed;
    [SerializeField] UnityEvent<HealthChangeInfo> OnDeath;

    private void Awake()
    {
        if(MaxHP <= MinHP)
        {
            Debug.LogError($"MaxHP must be greater than {MinHP} but is {MaxHP}", this);
            MaxHP = MinHP + 1f;
        }
        if (HP <= MinHP || HP > MaxHP) ResetHealth();
            
    }

    public bool IsDead => HP <= MinHP;

    [System.Serializable] public struct HealthChangeInfo
    {
        public static HealthChangeInfo Compute(Damageable damageable, float delta) => new (){ Damageable = damageable, OriginalHP = damageable.HP, RequestedDeltaHP = delta, DidDie = !damageable.IsDead && (damageable.HP + delta) <= MinHP };
        public Damageable Damageable { get; init; }
        public float OriginalHP { get; init; }
        public float RequestedDeltaHP { get; init; }
        public float ActualDeltaHP => ResultHP - OriginalHP;
        public float ResultHP => Mathf.Clamp(OriginalHP + RequestedDeltaHP, Damageable.MinHP, Damageable.MaxHP);
        public bool DidDie { get; init; }
    }

    public void ResetHealth()
    {
        HP = MaxHP;
    }

    public void Heal(float amount) => ChangeHP(amount, OnHealed);
    public void Damage(float amount) => ChangeHP(-amount, OnDamaged);

    private void ChangeHP(float amount, UnityEvent<HealthChangeInfo> @event)
    {
        if (IsDead)
        {
            Debug.Log($"Ignoring health change for already dead entity '{name}'", this);
            return;
        }

        var info = HealthChangeInfo.Compute(this, amount);
        this.HP = info.ResultHP;
        Debug.Log($"{name} damaged for {amount}({info.ActualDeltaHP}) HP -> now has {this.HP} HP (originally had {info.OriginalHP})");

        @event?.Invoke(info);
        if (info.DidDie)
        {
            OnDeath?.Invoke(info);
        }
    }
}
