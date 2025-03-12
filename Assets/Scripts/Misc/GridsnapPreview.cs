using MarkusSecundus.Utils.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridsnapPreview : MonoBehaviour
{
    public interface IPreviewable
    {
        public void NotifyIsValidPlacement(bool isValidPlacement);
    }

    [SerializeField] SnapToGrid _objectToSnap;
    [SerializeField] Transform _previewPivot;
    IPreviewable[] _notificationReceivers;
    
    private void Start()
    {
        _notificationReceivers = _previewPivot.GetComponentsInChildren<IPreviewable>(true);
    }

    private void Update()
    {
        _previewPivot.SnapWithPivot(_previewPivot, _objectToSnap.GetGridPosition());
        bool isValidPlacement = _objectToSnap.CheckHasPlaceOnGrid();
        foreach (var r in _notificationReceivers) r.NotifyIsValidPlacement(isValidPlacement);
    }
}
