using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class Damageable : MonoBehaviour
{
    public static Damageable Get(Object o) => o.GetComponentInParent<Damageable>();


    [SerializeField] float _maxHp;
    public float MaxHP => _maxHp;
    public const float MinHP = 0;

    public float HP { get; private set; }

    [SerializeField] UnityEvent<HealthChangeInfo> OnDamaged;
    [SerializeField] UnityEvent<HealthChangeInfo> OnHealed;
    [SerializeField] UnityEvent<HealthChangeInfo> OnDeath;

    [System.Serializable] public struct HealthChangeInfo
    {
        public static HealthChangeInfo Compute(Damageable damageable, float delta) => new (){ Damageable = damageable, OriginalHP = damageable.HP, RequestedDeltaHP = delta };
        public Damageable Damageable;
        public float OriginalHP;
        public float RequestedDeltaHP;
        public float ActualDeltaHP => ResultHP - OriginalHP;
        public float ResultHP => Mathf.Clamp(OriginalHP + RequestedDeltaHP, Damageable.MinHP, Damageable.MaxHP);
        public bool DidDie => ResultHP <= 0f;
    }


    public void Heal(float amount) => ChangeHP(amount, OnHealed);
    public void Damage(float amount) => ChangeHP(-amount, OnDamaged);

    private void ChangeHP(float amount, UnityEvent<HealthChangeInfo> @event)
    {
        var info = HealthChangeInfo.Compute(this, amount);
        this.HP = info.ResultHP;

        @event?.Invoke(info);
        if (info.DidDie)
            OnDeath?.Invoke(info);
    }
}
