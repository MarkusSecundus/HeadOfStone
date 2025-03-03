using MarkusSecundus.Utils.Debugging;
using MarkusSecundus.Utils.Primitives;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    [SerializeField] Grid _grid;

    [SerializeField] Grid.PlacementDescriptor _placement;

    private void OnDrawGizmos()
    {
        if (!_grid) return;

        TestPlacement(); 
        TestSinglePoint();
    }

    void TestPlacement()
    {
        var coords = _grid.GetGridCoords(_placement);
        var center = _grid.GetGridPosition(_placement);
        using (GizmoHelpers.ColorScope(Color.red)) Gizmos.DrawSphere(center, 0.5f);
        using (GizmoHelpers.ColorScope(Color.yellow)) foreach(var coord in coords.IterateValuesInclusive()) Gizmos.DrawSphere(_grid.GetGridPointCenter(coord), 0.2f);

        using (GizmoHelpers.ColorScope(Color.red)) _placement.DrawGizmo();
    }

    void TestSinglePoint()
    {
        var coords = _grid.GetGridCoords(transform.position);
        var center = _grid.GetGridPointCenter(coords);
        var origin = _grid.GetGridPointOrigin(coords);
        using (GizmoHelpers.ColorScope(Color.red)) Gizmos.DrawSphere(center, 0.5f);
        using (GizmoHelpers.ColorScope(coords.x >= 0 ? (coords.y >= 0 ? Color.yellow : Color.green) : (coords.y >= 0 ? Color.blue : Color.cyan))) Gizmos.DrawSphere(origin, 0.3f);
    }
}
