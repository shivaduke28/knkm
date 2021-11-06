using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoshinoLabs.IwaSync3
{
    public class DesktopBar : IwaSync3Base
    {
#pragma warning disable CS0414
        [SerializeField]
        IwaSync3 iwaSync3;

        [SerializeField]
        bool desktopOnly = true;
#pragma warning restore CS0414
    }
}
