using Assets.Scripts.DamageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkusSecundus.Utils.Extensions;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] float ShootForce;
    [SerializeField] float Cooldown_seconds = 0.3f;
    [SerializeField] Transform ShootDirection;
    [SerializeField] Rigidbody BulletPrototype;

    [SerializeField] KeyCode KeyToShoot = KeyCode.Mouse0;
    IInputProvider _input;
    WeaponDescriptor _weaponDescriptor;
    private void Start()
    {
        _input = IInputProvider.Get(this);
        _weaponDescriptor = GetComponentInParent<WeaponDescriptor>();
    }

    private void Update()
    {
        if (_input.GetKeyDown(KeyToShoot))
            if (_weaponDescriptor.AddAmmo(-1))
                DoShoot();
            else
                Debug.Log($"Insufficient ammo! ({_weaponDescriptor.CurrentAmmo})", this);
    }

    double _nextPermittedShotTime = double.NegativeInfinity;
    void DoShoot()
    {
        if (Time.timeAsDouble < _nextPermittedShotTime)
            return;
        _nextPermittedShotTime = Time.timeAsDouble + Cooldown_seconds;

        var bullet = Instantiate(BulletPrototype, null);
        bullet.transform.position = BulletPrototype.transform.position;
        bullet.transform.rotation = BulletPrototype.transform.rotation;
        bullet.gameObject.SetActive(true);

        if (ShootDirection && !bullet.isKinematic)
        {
            var shootForce = (ShootDirection.position - transform.position).normalized * ShootForce;
            bullet.AddForce(shootForce, ForceMode.Impulse);
        }
    }
}
