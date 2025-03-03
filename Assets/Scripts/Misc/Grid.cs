using JetBrains.Annotations;
using MarkusSecundus.Utils.Datastructs;
using MarkusSecundus.Utils.Debugging;
using MarkusSecundus.Utils.Extensions;
using MarkusSecundus.Utils.Graphics;
using MarkusSecundus.Utils.Primitives;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class Grid : MonoBehaviour
{
    [System.Serializable] public struct PlacementDescriptor
    {
        [field:SerializeField] public Transform Pivot { get; private set; }
        [field: SerializeField] public Vector2 Dimensions { get; private set; }

        public PlacementDescriptor(Transform pivot, Vector2? dimensions = null) => (Pivot, Dimensions) = (pivot, dimensions ?? Vector2.zero);

        //public static implicit operator PlacementDescriptor(Transform pivot) => new PlacementDescriptor(pivot);

        public void DrawGizmo() => DrawHelpers.DrawLineSquare(Dimensions, Pivot, Gizmos.DrawLine);


        public static PlacementDescriptor Default => default;
    }


    [field: SerializeField] public Vector2 Dimensions { get; private set; } = new Vector2(1f, 1f);


    DefaultValDict<Vector2Int, List<Transform>> _objectsOnGrid = new(o => new ());




    Vector3 VectorLocalToGlobal(Vector2 local)
    {
        Vector3 ret = new Vector3(local.x, 0f, local.y);
        ret = transform.LocalToGlobal(ret);
        return ret;
    }

    public Vector2Int GetGridCoords(Vector3 pos)
    {
        pos = transform.GlobalToLocal(pos);
        return new Vector2Int(Mathf.FloorToInt(pos.x / Dimensions.x), Mathf.FloorToInt(pos.z / Dimensions.y));
    }


    public bool TryGetObjectsOnGridPoint(Vector3 worldCoords, out IReadOnlyList<Transform> objects)
    {
        return TryGetObjectsOnGridPoint(GetGridCoords(worldCoords), out objects);
    }
    public bool TryGetObjectsOnGridPoint(Vector2Int gridCoords, out IReadOnlyList<Transform> objects)
    {
        objects = default;
        if (_objectsOnGrid.TryGetValue(gridCoords, out var ret))
        {
            objects = ret;
            return true;
        }
        return false;
    }

    public Vector3 GetGridPointOrigin(Vector2Int gridCoords)
    {
        var ret = Dimensions.MultiplyElems(gridCoords.x, gridCoords.y) ;
        return VectorLocalToGlobal(ret);
    }
    public Vector3 GetGridPointEnd(Vector2Int gridCoords)
    {
        var ret = Dimensions.MultiplyElems(gridCoords.x, gridCoords.y) + Dimensions * 1f;
        return VectorLocalToGlobal(ret);
    }
    public Vector3 GetGridPointCenter(Vector2Int gridCoords)
    {
        var ret = Dimensions.MultiplyElems(gridCoords.x, gridCoords.y) + Dimensions * 0.5f;
        return VectorLocalToGlobal(ret);
    }

    public Interval<Vector2Int> GetGridCoords(PlacementDescriptor placement)
    {
        return new Interval<Vector2Int>(GetGridCoords(placement.Pivot.LocalToGlobal(placement.Pivot.localPosition -placement.Dimensions.x0y() * 0.5f)), GetGridCoords(placement.Pivot.LocalToGlobal(placement.Pivot.localPosition + placement.Dimensions.x0y() * 0.5f)));
    }
    public Vector3 GetGridPosition(PlacementDescriptor placement)
    {
        if (placement.Dimensions == Vector2.zero) return GetGridPointCenter(GetGridCoords(placement.Pivot.position)); //fast path where the placed object is just a point
        var gridCoords = GetGridCoords(placement);
        return (GetGridPointOrigin(gridCoords.Min) + GetGridPointEnd(gridCoords.Max)) * 0.5f;
    }

    public void RegisterToGrid(Transform obj, Vector2Int? gridCoords=null)
    {
        gridCoords ??= GetGridCoords(obj.position);
        _objectsOnGrid[gridCoords.Value].Add(obj);
    }

    public void RegisterToGrid(PlacementDescriptor placement)
    {
        foreach (var coord in GetGridCoords(placement).IterateValuesInclusive()) RegisterToGrid(placement.Pivot, coord);
    }


    public bool UnregisterFromGrid(Transform obj, Vector2Int? gridCoords=null)
    {
        gridCoords ??= GetGridCoords(obj.position);
        if(_objectsOnGrid.TryGetValue(gridCoords.Value, out var list))
        {
            var didFind = list.Remove(obj);
            if (list.Count <= 0) _objectsOnGrid.Remove(gridCoords.Value);
            return didFind;
        }
        return false;
    }
    public void UnregisterFromGrid(PlacementDescriptor placement)
    {
        foreach (var coord in GetGridCoords(placement).IterateValuesInclusive()) UnregisterFromGrid(placement.Pivot, coord);
    }


    [SerializeField] bool _shouldDrawGizmos = false;
    private void OnDrawGizmos()
    {
        if (!_shouldDrawGizmos) return;

        using (GizmoHelpers.ColorScope(Color.green))
        {
            const int SIZE = 20;
            for (int t = -SIZE; t < SIZE; ++t)
            {
                Gizmos.DrawLine(GetGridPointOrigin(new Vector2Int(t, -SIZE)), GetGridPointOrigin(new Vector2Int(t, SIZE)));
                Gizmos.DrawLine(GetGridPointOrigin(new Vector2Int(-SIZE, t)), GetGridPointOrigin(new Vector2Int(SIZE, t)));
            }
        }
    }
}
