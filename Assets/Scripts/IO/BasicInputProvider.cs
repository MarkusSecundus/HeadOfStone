﻿using MarkusSecundus.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.IO
{
    public class BasicInputProvider : MonoBehaviour, IInputProvider
    {
        [SerializeField] Camera _camera;
        new Camera camera => _camera = _camera.IfNil(Camera.main);

        public float GetAxis(InputAxis axis) => Input.GetAxis(axis.GetName());
        public float GetAxisRaw(InputAxis axis) => Input.GetAxisRaw(axis.GetName());

        public bool GetKey(KeyCode c) => Input.GetKey(c);

        public bool GetKeyDown(KeyCode c) => Input.GetKeyDown(c);

        public bool GetKeyUp(KeyCode c) => Input.GetKeyUp(c);

        public Ray GetMouseRay()
        {
            var ret = camera.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(ret.origin, ret.direction*10f, Color.yellow);
            return ret;
        }
    }
}
