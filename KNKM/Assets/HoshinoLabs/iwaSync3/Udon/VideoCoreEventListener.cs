﻿using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components.Video;
using VRC.SDKBase;
using VRC.Udon;

namespace HoshinoLabs.Udon.IwaSync3
{
    public abstract class VideoCoreEventListener : UdonSharpBehaviour
    {
        #region VideoEvent
        public override void OnVideoEnd() { }
        public override void OnVideoError(VideoError videoError) { }
        public override void OnVideoLoop() { }
        public override void OnVideoReady() { }
        public override void OnVideoStart() { }
        #endregion

        #region VideoCoreEvent
        public void OnPlayerPlay() { }
        public void OnPlayerPause() { }
        public void OnPlayerStop() { }

        public void OnChangeURL() { }
        public void OnChangeLoop() { }
        #endregion
    }
}
