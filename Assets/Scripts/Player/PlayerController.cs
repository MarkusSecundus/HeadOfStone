using Assets.Scripts.IO;
using MarkusSecundus.Utils.Geometry;
using MarkusSecundus.Utils.Graphics;
using MarkusSecundus.Utils.Input;
using MarkusSecundus.Utils.Primitives;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float movementSpeed;
    [SerializeField] float movementInertia = 1f;
    [SerializeField] float rotationInertia = 1f;
    [SerializeField] Vector3 gravity = new Vector3(0, -9.81f, 0);
    [SerializeField] Transform bodyToRotate;

    [SerializeField] UnityEvent OnWalking;

    IInputProvider<InputAxis> input;
    CharacterController controller;

    void Start()
    {
        input = IInputProvider<InputAxis>.Get(this);
        controller = GetComponent<CharacterController>();
    }




    Vector3 _movementVelocity = Vector3.zero;
    Vector3 _gravitationalVelocity = Vector3.zero;
    Vector3 _totalVelocity => _movementVelocity + _gravitationalVelocity;
    void Update()
    {
        var delta = Time.deltaTime;
        VelocityUpdate(GetTargetVelocity(), delta);
        RotationUpdate(GetTargetRotation(), delta);
    }



    void VelocityUpdate(Vector3 newTargetVelocity, float delta)
    {
        _movementVelocity = Vector3.Lerp(_movementVelocity, newTargetVelocity, Mathf.Pow(delta, movementInertia));

        if (controller.isGrounded)
            _gravitationalVelocity = Vector3.zero;
        else
            _gravitationalVelocity += gravity * delta;

        _movementVelocity = _movementVelocity.ClampMagnitude(0f, movementSpeed);
        if (_movementVelocity.sqrMagnitude > (movementSpeed * 0.1f))
            OnWalking?.Invoke();
        controller.Move(_totalVelocity * delta);
    }
    void RotationUpdate(Quaternion? newTargetRotation, float delta)
    {
        if (newTargetRotation is not Quaternion newRotation) return;
        bodyToRotate.rotation = Quaternion.Lerp(bodyToRotate.rotation, newRotation, Mathf.Pow(delta, rotationInertia));
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
        if (inputRay.Intersect(new Plane(Vector3.up, transform.position)) is Vector3 lookPoint)
        {
            //Debug.DrawRay(lookPoint, Vector3.up * 100, Color.blue);
            return Quaternion.LookRotation((lookPoint - transform.position).With(y: 0));
        }
        return null;
    }


}
