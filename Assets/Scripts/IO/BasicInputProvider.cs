using MarkusSecundus.Utils.Extensions;
using MarkusSecundus.Utils.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.IO
{
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


    public class BasicInputProvider : BasicInputProviderBase<InputAxis>
    {
        protected override string GetAxisName(InputAxis axis) => axis.GetName();
    }
}
