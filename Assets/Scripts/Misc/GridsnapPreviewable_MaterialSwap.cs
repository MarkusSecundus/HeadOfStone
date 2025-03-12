using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridsnapPreviewable_MaterialSwap : MonoBehaviour, GridsnapPreview.IPreviewable
{
    [SerializeField] Material _onValid;
    [SerializeField] Material _onInvalid;

    [SerializeField] Renderer _rendererToModify;

    public void NotifyIsValidPlacement(bool isValidPlacement)
    {
        _rendererToModify.material = isValidPlacement? _onValid: _onInvalid;
    }
}
