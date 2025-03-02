using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour, ProjectileShooter.IProjectile
{
    [SerializeField] KeyCode _detonateKey;
    [SerializeField] ExplosionEffect _explosion;

    IInputProvider _input;

    public void OnShot(ProjectileShooter weapon)
    {
        _input = IInputProvider.Get(weapon);
    }

    private void Update()
    {
        if (_input.GetKeyDown(_detonateKey))
        {
            _explosion.RunExplosionEffect();
        }
    }
}
