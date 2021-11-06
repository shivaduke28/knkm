using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VRC.SDK3.Video.Components.AVPro.VRCAVProVideoSpeaker;

namespace HoshinoLabs.IwaSync3
{
    public class Speaker : IwaSync3Base
    {
#pragma warning disable CS0414
        [SerializeField]
        IwaSync3 iwaSync3;

        [SerializeField]
        float maxDistance = 12f;

        [SerializeField]
        bool spatialize = false;

        [SerializeField]
        ChannelMode mode;
#pragma warning restore CS0414

        public IwaSync3 mainIwaSync3
        {
            get
            {
                var iwaSync3 = GetComponentInParent<IwaSync3>();
                if (iwaSync3)
                    return iwaSync3;
                if (FindObjectsOfType<IwaSync3>().Length == 1)
                    return FindObjectOfType<IwaSync3>();
                return this.iwaSync3;
            }
        }
    }
}
