using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Vector3 speedForward;
    [SerializeField] Vector3 speedRight;

    IInputProvider input;
    CharacterController controller;

    void Start()
    {
        input = IInputProvider.Get(this);
        controller = GetComponent<CharacterController>();
    }


    void Update()
    {
        var delta = Time.deltaTime;
        Vector3 toMove = speedForward * input.GetAxis(InputAxis.Vertical)
                       + speedRight * input.GetAxis(InputAxis.Horizontal);
        toMove *= delta;

        controller.Move(toMove);
    }
}
