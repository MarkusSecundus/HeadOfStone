using MarkusSecundus.Utils.Primitives;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileGridPlacer : MonoBehaviour, ProjectileShooter.IProjectile
{
    SnapToGrid _snap;
    SnapToGrid Snap => _snap ??= GetComponent<SnapToGrid>();

    public void OnShot(ProjectileShooter weapon)
    {
        Snap.DoSnap();
    }

    bool ProjectileShooter.IProjectile.CheckCanShoot()
    {
        return Snap.CheckHasPlaceOnGrid();
    }
}
