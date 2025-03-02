using MarkusSecundus.Utils.Datastructs;
using MarkusSecundus.Utils.Debugging;
using MarkusSecundus.Utils.Extensions;
using MarkusSecundus.Utils.Primitives;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [field: SerializeField] public Vector2 Dimensions { get; private set; } = new Vector2(1f, 1f);
    [field: SerializeField] public VectorField.FieldType NormalDimension => VectorField.FieldType.Y;
    [field: SerializeField] public float NormalPosition => 0f;

    public int Index0 => NormalDimension.ToIndex() == 0 ? 1 : 0;
    public int Index1 => NormalDimension.ToIndex() == 2 ? 1 : 2;

    DefaultValDict<Vector2Int, List<Transform>> _objectsOnGrid = new(o => new ());


    Vector3 MakeVector3(float _0, float _1, float normal)
    {
        Vector3 ret = default;
        ret[Index0] = _0;
        ret[Index1] = _1;
        ret[NormalDimension.ToIndex()] = normal;
        ret = transform.LocalToGlobal(ret);
        return ret;
    }

    public Vector2Int GetGridCoords(Vector3 pos)
    {
        pos = transform.GlobalToLocal(pos);
        return new Vector2Int((int)(pos[Index0] / Dimensions[0]), (int)(pos[Index1] / Dimensions[1]));
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
        var ret = Dimensions.MultiplyElems(gridCoords.x, gridCoords.y);
        return MakeVector3(ret.x, ret.y, NormalPosition);
    }
    public Vector3 GetGridPointCenter(Vector2Int gridCoords)
    {
        var ret = Dimensions.MultiplyElems(gridCoords.x, gridCoords.y) - Dimensions * 0.5f;
        return MakeVector3(ret.x, ret.y, NormalPosition);
    }

    public void RegisterToGrid(Transform obj, Vector2Int? gridCoords=null)
    {
        gridCoords ??= GetGridCoords(obj.position);
        _objectsOnGrid[gridCoords.Value].Add(obj);
    }


    public void UnregisterFromGrid(Transform obj, Vector2Int? gridCoords=null)
    {
        gridCoords ??= GetGridCoords(obj.position);
        if(_objectsOnGrid.TryGetValue(gridCoords.Value, out var list))
        {
            list.Remove(obj);
            if (list.Count <= 0) _objectsOnGrid.Remove(gridCoords.Value);
        }
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
                Gizmos.DrawLine(GetGridPointCenter(new Vector2Int(t, -SIZE)), GetGridPointCenter(new Vector2Int(t, SIZE)));
                Gizmos.DrawLine(GetGridPointCenter(new Vector2Int(-SIZE, t)), GetGridPointCenter(new Vector2Int(SIZE, t)));
            }
        }
    }
}
