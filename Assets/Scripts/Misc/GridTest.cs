using MarkusSecundus.Utils.Debugging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    [SerializeField] Grid _grid;


    private void OnDrawGizmos()
    {
        if (!_grid) return;

        var coords = _grid.GetGridCoords(transform.position);
        var center = _grid.GetGridPointOrigin(coords);
        using (GizmoHelpers.ColorScope(Color.red))
        {
            Gizmos.DrawSphere(center, 0.5f);
        }
    }
}
