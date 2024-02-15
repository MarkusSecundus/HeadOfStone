using MarkusSecundus.PhysicsSwordfight.Utils.Geometry;
using MarkusSecundus.PhysicsSwordfight.Utils.Graphics;
using MarkusSecundus.PhysicsSwordfight.Utils.Primitives;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed;

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
        Vector3 toMove = Vector3.forward * input.GetAxisRaw(InputAxis.Vertical)
                       + Vector3.right * input.GetAxisRaw(InputAxis.Horizontal);
        toMove = toMove.normalized * movementSpeed * delta;

        var inputRay = input.GetMouseRay();
        if(inputRay.Intersect(new Plane(Vector3.up, transform.position.y)) is Vector3 lookPoint)
        {
            var rotation = Quaternion.LookRotation((lookPoint - transform.position).With(y:0));
            transform.rotation = rotation;
        }


        controller.Move(toMove);
    }


}
