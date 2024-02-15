using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum InputAxis
{
    Horizontal,
    Vertical,
    MouseX,
    MouseY
}
public static class InputAxisExtensions
{
    public static string GetName(this InputAxis a) => a.ToString();
}

public interface IInputProvider
{
    public float GetAxis(InputAxis axis);
    public float GetAxisRaw(InputAxis axis);

    public Ray GetMouseRay();

    public bool GetKey(KeyCode c);
    public bool GetKeyDown(KeyCode c);
    public bool GetKeyUp(KeyCode c);


    public static IInputProvider Get(Object o) => o.GetComponentInParent<IInputProvider>();
}