using Assets.Scripts.DamageSystem;
using Assets.Scripts.IO;
using MarkusSecundus.Utils.Datastructs;
using MarkusSecundus.Utils.Extensions;
using MarkusSecundus.Utils.Input;
using MarkusSecundus.Utils.Physics;
using MarkusSecundus.Utils.Primitives;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public abstract class ContinuousShapecastShooterBase : MonoBehaviour
{
    [SerializeField] float DamagePerSecond;
    [SerializeField] DamageType DamageType;

    [SerializeField] ColliderActivityTracker _shape;


    [SerializeField] ParticleSystem _particles;

    protected abstract float Intensity { get; }

    DefaultValDict<Damageable, HashSet<IArmorPiece>> _affectedDamageables = new(d=>new());

    record ShapeListener(ContinuousShapecastShooterBase Base) : IColliderActivityInfo
    {
        public void Enter(Collider other)
        {
            IArmorPiece armor = IArmorPiece.Get(other);
            if (armor.IsNil()) return;
            Base._affectedDamageables[armor.Damageable].Add(armor);
        }

        public void Exit(Collider other)
        {
            IArmorPiece armor = IArmorPiece.Get(other);
            if (armor.IsNil()) return;
            if (!Base._affectedDamageables.TryGetValue(armor.Damageable, out var armors)) return;
            armors.Remove(armor);
            if (armors.Count <= 0) Base._affectedDamageables.Remove(armor.Damageable);
        }
    }

    ShapeListener _shapeListener;

    float _maxEmissionRate;
    protected virtual void Start()
    {
        if(_shape) _shape.RegisterListener(_shapeListener = new(this));
        if(_particles)
        { 
            var emission = _particles.emission;
            _maxEmissionRate = emission.rateOverTimeMultiplier;
            emission.rateOverTimeMultiplier = 0f;
        }
    }

    private void OnDestroy()
    {
        if(_shape) _shape.UnregisterListener(_shapeListener);
    }

    protected virtual void Update()
    {
        if(_particles)
        { 
            var emission = _particles.emission;
            emission.rateOverTimeMultiplier = _maxEmissionRate * Intensity;
        }

        if (Intensity <= 0f) return;
        if (_affectedDamageables.IsEmpty()) return;

        float damage = DamagePerSecond * Intensity * Time.deltaTime;

        HashSet<Damageable> deadEntities = null;

        foreach(var (entity, armors) in _affectedDamageables)
        {
            if (!entity)
            {
                (deadEntities ??= new()).Add(entity);
                continue;
            }

            float damagePerPiece = damage / armors.Count;
            if (damagePerPiece == 0f) continue;
            foreach(var armor in armors)
                armor.Attack(new AttackDeclaration { Attacker = this, Damage = damagePerPiece, Type = DamageType });
        }
        if(deadEntities != null) foreach (var dead in deadEntities) _affectedDamageables.Remove(dead);

    }
}



public class ContinuousShapecastShooter : ContinuousShapecastShooterBase
{
    [SerializeField] float _buildupTime_seconds;
    [SerializeField] float _fadeTime_seconds;
    [SerializeField] float AmmoPerSecond;

    [SerializeField] KeyCode _triggerKey = KeyCode.Mouse0;
    IInputProvider<InputAxis> _input;
    WeaponDescriptor _weapon;



    float _intensity = 0f;
    protected override float Intensity => _intensity;

    protected override void Start()
    {
        base.Start();
        _weapon = GetComponent<WeaponDescriptor>();
        _input = IInputProvider<InputAxis>.Get(this);
    }

    protected override void Update()
    {
        bool isActivated = _input.GetKey(_triggerKey) && _weapon.AddAmmo(-AmmoPerSecond * Time.deltaTime);
        float change = (1f / (isActivated ? _buildupTime_seconds : -_fadeTime_seconds));
        _intensity = (_intensity + change * Time.deltaTime).Clamp01();

        base.Update();
    }
}