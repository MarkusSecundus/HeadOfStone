using MarkusSecundus.Utils.Behaviors.GameObjects;
using MarkusSecundus.Utils.Debugging;
using MarkusSecundus.Utils.Extensions;
using MarkusSecundus.Utils.Primitives;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class SnapToGrid : MonoBehaviour
{
    [SerializeField] bool _shouldSnapOnStart = false;
    [SerializeField] Grid.PlacementDescriptor _placement;
    public Grid.PlacementDescriptor Placement => _placement.Pivot ? _placement: _placement.WithPivot(transform);

    [SerializeField] Grid _grid;
    public Grid Grid => _grid ? _grid : (TagSearchable.FindByTag<Grid>(_gridTag) ?? GameObject.FindFirstObjectByType<Grid>());
    [SerializeField] string _gridTag;

    [SerializeField] bool _shouldRegisterToGrid = false;

    void Awake()
    {
        _placement = Placement;
        _grid = Grid;
    }

    private void Start()
    {
        if (_shouldSnapOnStart) DoSnap();
    }

    public void DoSnap()
    {
        if (Grid)
        {
            transform.SnapWithPivot(Placement.Pivot, Grid.GetGridPosition(Placement));

            if (_shouldRegisterToGrid) Grid.RegisterToGrid(Placement);
        }
    }
    public bool CheckHasPlaceOnGrid()
    {
        if (Grid)
        {
            return Grid.GetGridCoords(Placement).IterateValuesInclusive().All(p => !Grid.TryGetObjectsOnGridPoint(p, out _));
        }
        return false;
    }

    private void OnDestroy()
    {
        if (Grid && _shouldRegisterToGrid) Grid.UnregisterFromGrid(Placement);
    }


    [SerializeField] bool _shouldDrawGizmo;
    private void OnDrawGizmos()
    {
        if(_shouldDrawGizmo) using (GizmoHelpers.ColorScope(Color.cyan)) Placement.DrawGizmo();
    }
}
