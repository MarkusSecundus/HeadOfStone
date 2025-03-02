using Assets.Scripts.IO;
using MarkusSecundus.Utils.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInputProvider : AbstractRedirectedInputProvider<InputAxis>, ProjectileShooter.IProjectile
{
    IInputProvider<InputAxis> _sourceField;
    protected override IInputProvider<InputAxis> _source => _sourceField;

    public void OnShot(ProjectileShooter weapon)
    {
        _sourceField = IInputProvider<InputAxis>.Get(weapon);
    }
}
