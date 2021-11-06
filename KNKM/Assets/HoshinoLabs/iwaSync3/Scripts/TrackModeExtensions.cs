﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HoshinoLabs.IwaSync3
{
    public static class TrackModeExtensions
    {
        public static uint ToVideoCoreMode(this TrackMode self)
        {
            return self == TrackMode.Video ? Udon.IwaSync3.VideoCore.MODE_VIDEO : Udon.IwaSync3.VideoCore.MODE_STREAM;
        }
    }
}
