using MarkusSecundus.Utils.Primitives;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileGridPlacer : MonoBehaviour, ProjectileShooterBase.IProjectile
{
    SnapToGrid _snap;
    SnapToGrid Snap => _snap ??= GetComponent<SnapToGrid>();

    public void OnShot(ProjectileShooterBase weapon)
    {
        Snap.DoSnap();
    }

    bool ProjectileShooterBase.IProjectile.CheckCanShoot()
    {
        return Snap.CheckHasPlaceOnGrid();
    }
}
