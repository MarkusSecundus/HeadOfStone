using MarkusSecundus.Utils.Behaviors.GameObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunpowderBehavior : MonoBehaviour, ProjectileShooter.IProjectile
{
    [SerializeField] string _gridTag;

    Grid _grid;
    private void Start()
    {
        _grid = TagSearchable.FindByTag(_gridTag).GetComponent<Grid>();
    }

    private void OnDestroy()
    {
        if (_grid) _grid.UnregisterFromGrid(transform);
    }

    public void OnShot(ProjectileShooter weapon)
    {
        if (_grid) _grid.RegisterToGrid(transform);
    }

    bool ProjectileShooter.IProjectile.CheckCanShoot()
    {
        return !_grid.TryGetObjectsOnGridPoint(transform.position, out _);
    }
}
