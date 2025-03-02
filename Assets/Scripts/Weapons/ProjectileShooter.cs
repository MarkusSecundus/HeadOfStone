using Assets.Scripts.DamageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkusSecundus.Utils.Extensions;
using Assets.Scripts.IO;
using MarkusSecundus.Utils.Input;



public class ProjectileShooter : MonoBehaviour
{
    public interface IProjectile
    {
        public void OnShot(ProjectileShooter weapon);
    }

    [SerializeField] float ShootForce;
    [SerializeField] float Cooldown_seconds = 0.3f;
    [SerializeField] int MaxProjectilesInExistence = -1;
    [SerializeField] Transform ShootDirection;
    [SerializeField] Rigidbody BulletPrototype;

    [SerializeField] KeyCode KeyToShoot = KeyCode.Mouse0;
    IInputProvider<InputAxis> _input;
    WeaponDescriptor _weaponDescriptor;


    LinkedList<Rigidbody> _projectiles = new();

    void _removeDeadProjectiles()
    {
        for (var node = _projectiles.First; node != null; )
        {
            var next = node.Next;
            if(node.Value.IsNil()) node.List.Remove(node);
            node = next;
        }
    }

    private void Start()
    {
        _input = IInputProvider<InputAxis>.Get(this);
        _weaponDescriptor = GetComponentInParent<WeaponDescriptor>();

        StartCoroutine(deadProjectileRemover());
        IEnumerator deadProjectileRemover()
        {
            while (true)
            {
                _removeDeadProjectiles();
                yield return new WaitForSeconds(2f);
            }
        }
    }

    private void Update()
    {
        if (_input.GetKeyDown(KeyToShoot))
                DoShoot();
    }

    double _nextPermittedShotTime = double.NegativeInfinity;
    void DoShoot()
    {
        _removeDeadProjectiles();
        if (MaxProjectilesInExistence >= 0 && _projectiles.Count >= MaxProjectilesInExistence)
            return;
        if (Time.timeAsDouble < _nextPermittedShotTime)
            return;
        _nextPermittedShotTime = Time.timeAsDouble + Cooldown_seconds;

        if (!_weaponDescriptor.AddAmmo(-1))
            return;

        var bullet = Instantiate(BulletPrototype, null);
        bullet.transform.position = BulletPrototype.transform.position;
        bullet.transform.rotation = BulletPrototype.transform.rotation;
        bullet.gameObject.SetActive(true);
        foreach (IProjectile script in bullet.GetComponentsInChildren<IProjectile>(true)) script.OnShot(this);

        _projectiles.AddLast(bullet);

        if (ShootDirection && !bullet.isKinematic)
        {
            var shootForce = (ShootDirection.position - transform.position).normalized * ShootForce;
            bullet.AddForce(shootForce, ForceMode.Impulse);
        }
    }
}
