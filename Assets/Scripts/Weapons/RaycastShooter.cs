using Assets.Scripts.DamageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkusSecundus.PhysicsSwordfight.Utils.Extensions;

public class RaycastShooter : MonoBehaviour
{
    [SerializeField] float ShootDistance;
    [SerializeField] float Cooldown_seconds = 0.3f;
    [SerializeField] Transform ShootDirection;
    

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


    }
}
