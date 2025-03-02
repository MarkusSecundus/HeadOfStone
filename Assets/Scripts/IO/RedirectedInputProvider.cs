using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class AbstractRedirectedInputProvider : AbstractInputProvider
{
    protected abstract IInputProvider _source { get; }

    public override float GetAxis(InputAxis axis) => _source?.GetAxis(axis) ?? 0f;

    public override float GetAxisRaw(InputAxis axis) => _source?.GetAxis(axis) ?? 0f;

    public override bool GetKey(KeyCode c) => _source?.GetKey(c) ?? false;

    public override bool GetKeyDown(KeyCode c) => _source?.GetKeyDown(c) ?? false;

    public override bool GetKeyUp(KeyCode c) => _source?.GetKeyUp(c) ?? false;

    public override Ray GetMouseRay() => _source?.GetMouseRay() ?? default;
}


public class RedirectedInputProvider : AbstractRedirectedInputProvider
{
    [SerializeField] AbstractInputProvider _sourceObject;
    protected override IInputProvider _source => _sourceObject;
}
