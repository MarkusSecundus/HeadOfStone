using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInputProvider : AbstractRedirectedInputProvider, ProjectileShooter.IProjectile
{
    IInputProvider _sourceField;
    protected override IInputProvider _source => _sourceField;

    public void OnShot(ProjectileShooter weapon)
    {
        _sourceField = IInputProvider.Get(weapon);
    }
}
