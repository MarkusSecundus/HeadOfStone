using MarkusSecundus.PhysicsSwordfight.Utils.Geometry;
using MarkusSecundus.PhysicsSwordfight.Utils.Graphics;
using MarkusSecundus.PhysicsSwordfight.Utils.Primitives;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float movementSpeed;
    [SerializeField] float movementInertia = 1f;
    [SerializeField] float rotationInertia = 1f;
    [SerializeField] Vector3 gravity = new Vector3(0, -9.81f, 0);

    IInputProvider input;
    CharacterController controller;

    void Start()
    {
        input = IInputProvider.Get(this);
        controller = GetComponent<CharacterController>();
    }




    Vector3 velocity = Vector3.zero;
    void Update()
    {
        var delta = Time.deltaTime;
        VelocityUpdate(GetTargetVelocity(), delta);
        RotationUpdate(GetTargetRotation(), delta);
    }



    void VelocityUpdate(Vector3 newTargetVelocity, float delta)
    {
        velocity = Vector3.Lerp(velocity, newTargetVelocity, Mathf.Pow(delta, movementInertia));
        if (!controller.isGrounded)
        {
            velocity += gravity * delta;
        }
        controller.Move(velocity);
    }
    void RotationUpdate(Quaternion? newTargetRotation, float delta)
    {
        if (newTargetRotation is not Quaternion newRotation) return;
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Mathf.Pow(delta, rotationInertia));
    }

    Vector3 GetTargetVelocity()
    {
        Vector3 targetVelocity = Vector3.forward * input.GetAxisRaw(InputAxis.Vertical)
                               + Vector3.right * input.GetAxisRaw(InputAxis.Horizontal);
        return targetVelocity.normalized * movementSpeed;
    }
    Quaternion? GetTargetRotation()
    {
        var inputRay = input.GetMouseRay();
        if (inputRay.Intersect(new Plane(Vector3.up, transform.position.y)) is Vector3 lookPoint)
            return Quaternion.LookRotation((lookPoint - transform.position).With(y: 0));
        return null;
    }


}
