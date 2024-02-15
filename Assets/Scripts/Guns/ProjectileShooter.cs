using Assets.Scripts.DamageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkusSecundus.PhysicsSwordfight.Utils.Extensions;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] float ShootForce;
    [SerializeField] float Cooldown_seconds = 0.3f;
    [SerializeField] Transform ShootDirection;
    [SerializeField] Rigidbody BulletPrototype;

    [SerializeField] KeyCode KeyToShoot = KeyCode.Mouse0;
    IInputProvider input;
    private void Start()
    {
        input = IInputProvider.Get(this);
    }

    private void Update()
    {
        if (input.GetKeyDown(KeyToShoot))
            DoShoot();
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

        var shootForce = (ShootDirection.position - transform.position).normalized * ShootForce;
        Debug.Log($"Shoot force: {shootForce}", this);
        bullet.AddForce(shootForce, ForceMode.Impulse); 
    }
}
