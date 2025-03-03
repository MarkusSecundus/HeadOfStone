using MarkusSecundus.Utils.Behaviors.GameObjects;
using MarkusSecundus.Utils.Debugging;
using MarkusSecundus.Utils.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SnapToGrid : MonoBehaviour
{
    [SerializeField] Grid.PlacementDescriptor _placement;

    [SerializeField] Grid _grid;
    [SerializeField] string _gridTag;

    [SerializeField] bool _shouldRegisterToGrid = false;

    void Start()
    {
        if (!_placement.Pivot) _placement = new Grid.PlacementDescriptor(transform, _placement.Dimensions);
        if (!_grid)  _grid = TagSearchable.FindByTag<Grid>(_gridTag);
        if (!_grid)  _grid = GameObject.FindFirstObjectByType<Grid>();
        if (_grid)
        {
            transform.SnapWithPivot(_placement.Pivot, _grid.GetGridPosition(_placement));

            if(_shouldRegisterToGrid) _grid.RegisterToGrid(_placement);
        }
    }

    private void OnDestroy()
    {
        if (_shouldRegisterToGrid) _grid.UnregisterFromGrid(_placement);
    }


    [SerializeField] bool _shouldDrawGizmo;
    private void OnDrawGizmos()
    {
        if(_shouldDrawGizmo) using (GizmoHelpers.ColorScope(Color.cyan)) _placement.DrawGizmo();
    }
}
