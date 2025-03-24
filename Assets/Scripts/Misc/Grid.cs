using JetBrains.Annotations;
using MarkusSecundus.Utils.Datastructs;
using MarkusSecundus.Utils.Debugging;
using MarkusSecundus.Utils.Extensions;
using MarkusSecundus.Utils.Graphics;
using MarkusSecundus.Utils.Primitives;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Grid : MonoBehaviour
{
    [System.Serializable] public struct PlacementDescriptor
    {
        [field:SerializeField] public Transform Pivot { get; private set; }
        [field: SerializeField] public Vector2 Dimensions { get; private set; }

        public PlacementDescriptor(Transform pivot, Vector2? dimensions = null) => (Pivot, Dimensions) = (pivot, dimensions ?? Vector2.zero);

        //public static implicit operator PlacementDescriptor(Transform pivot) => new PlacementDescriptor(pivot);

        public void DrawGizmo() => DrawHelpers.DrawWireSquare(Dimensions, Pivot, Gizmos.DrawLine);


        public void GetRectangleEdgePointsWorldspace(out Vector3 a, out Vector3 b, out Vector3 c, out Vector3 d)
        {
            a = Pivot.LocalToGlobal(Dimensions.With(x: -V.X, y: 0, z: -V.Y) * 0.5f);
            b = Pivot.LocalToGlobal(Dimensions.With(x:  V.X, y: 0, z:  V.Y) * 0.5f);
            c = Pivot.LocalToGlobal(Dimensions.With(x:  V.X, y: 0, z: -V.Y) * 0.5f);
            d = Pivot.LocalToGlobal(Dimensions.With(x: -V.X, y: 0, z:  V.Y) * 0.5f);
        }

        public static PlacementDescriptor Default => default;
        public PlacementDescriptor WithPivot(Transform newPivot) => new PlacementDescriptor(newPivot, Dimensions);
    }


    [field: SerializeField] public Vector2 Dimensions { get; private set; } = new Vector2(1f, 1f);


    DefaultValDict<Vector2Int, List<Transform>> _objectsOnGrid = new(o => new ());




    Vector3 VectorLocalToGlobal(Vector2 local)
    {
        Vector3 ret = new Vector3(local.x, 0f, local.y);
        ret = transform.LocalToGlobal(ret);
        return ret;
    }

    public Vector2Int GetRectSizeInGridPoints(PlacementDescriptor placement, out Vector2 remainder)
    {
        placement.GetRectangleEdgePointsWorldspace(out var aWorld, out var bWorld, out var cWorld, out var dWorld);
        Vector2 a = transform.GlobalToLocal(aWorld).xz(), b = transform.GlobalToLocal(bWorld).xz(), c = transform.GlobalToLocal(cWorld).xz(), d = transform.GlobalToLocal(dWorld).xz();
        Vector2 min = a.Min(b).Min(c).Min(d);
        Vector2 max = a.Max(b).Max(c).Max(d);
        Vector2 dims = max - min;
        
        var ret = new Vector2Int(dims.x.DivideWithRemainder(Dimensions.x, out remainder.x), dims.y.DivideWithRemainder(Dimensions.y, out remainder.y));
        ret += new Vector2Int((int)Mathf.Sign(remainder.x), (int)Mathf.Sign(remainder.y));
        remainder = Dimensions - remainder;
        return ret;
    }

    public Vector2Int GetGridCoords(Vector3 pos, out Vector2 offsetFromCellOrigin)
    {
        pos = transform.GlobalToLocal(pos);
        return new Vector2Int(pos.x.DivideWithRemainder(Dimensions.x, out offsetFromCellOrigin.x), pos.z.DivideWithRemainder(Dimensions.y, out offsetFromCellOrigin.y));
    }
    public Vector2Int GetGridCoords(Vector3 pos) => GetGridCoords(pos, out _);


    public bool TryGetObjectsOnGridPoint(Vector3 worldCoords, out IReadOnlyCollection<Transform> objects)
    {
        return TryGetObjectsOnGridPoint(GetGridCoords(worldCoords), out objects);
    }
    public bool TryGetObjectsOnGridPoint(Vector2Int gridCoords, out IReadOnlyCollection<Transform> objects)
    {
        objects = default;
        if (_objectsOnGrid.TryGetValue(gridCoords, out var ret) && ret.Count > 0)
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


    public static (int, T) Min<T>((int value, T additional) a, (int value, T additional) b)
    {
        return a.value <= b.value ? a : b;
    }
    public static (int, T) Max<T>((int value, T additional) a, (int value, T additional) b)
    {
        return a.value >= b.value ? a : b;
    }

    public Interval<Vector2Int> GetGridCoords(PlacementDescriptor placement)
    {
        placement.GetRectangleEdgePointsWorldspace(out var a, out var b, out var c, out var d);

        Vector2Int aGrid = GetGridCoords(a, out var offsetA), bGrid = GetGridCoords(b, out var offsetB);
        Vector2Int cGrid = GetGridCoords(c, out var offsetC), dGrid = GetGridCoords(d, out var offsetD);
        var ret = Interval.Make(aGrid.Min(bGrid).Min(cGrid).Min(dGrid), aGrid.Max(bGrid).Max(cGrid).Max(dGrid));


#if false
        var (minX, minOffsetX) = Min((aGrid.x, offsetA), Min((bGrid.x, offsetB), Min((cGrid.x, offsetC), (dGrid.x, offsetD))));
        var (minY, minOffsetY) = Min((aGrid.y, offsetA), Min((bGrid.y, offsetB), Min((cGrid.y, offsetC), (dGrid.y, offsetD))));
        var (maxX, maxOffsetX) = Max((aGrid.x, offsetA), Max((bGrid.x, offsetB), Max((cGrid.x, offsetC), (dGrid.x, offsetD))));
        var (maxY, maxOffsetY) = Max((aGrid.y, offsetA), Max((bGrid.y, offsetB), Max((cGrid.y, offsetC), (dGrid.y, offsetD))));
        
        Vector2 minOffset = minOffsetX.Min(minOffsetY);
        Vector2 maxOffset = Dimensions - maxOffsetX.Max(maxOffsetY);

        if (maxOffset.x + minOffset.x >= Dimensions.x) { 
            if (maxOffset.x > minOffset.x) 
                ret.Max.x -= 1; 
            else ret.Min.x += 1; 
        }
        if (maxOffset.y + minOffset.y >= Dimensions.y) {
            if (maxOffset.y > minOffset.y) 
                ret.Max.y -= 1; 
            else ret.Min.y += 1; 
        }
        //Debug.Log($"{maxOffset}, {minOffset}");
#endif
        return ret;
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
            const int SIZE = 20, OVERLAPPED = SIZE + 3;
            for (int t = -SIZE; t <= SIZE; ++t)
            {
                Gizmos.DrawLine(GetGridPointOrigin(new Vector2Int(t, -OVERLAPPED)), GetGridPointOrigin(new Vector2Int(t, OVERLAPPED)));
                Gizmos.DrawLine(GetGridPointOrigin(new Vector2Int(-OVERLAPPED, t)), GetGridPointOrigin(new Vector2Int(OVERLAPPED, t)));
            }
        }
    }
}
