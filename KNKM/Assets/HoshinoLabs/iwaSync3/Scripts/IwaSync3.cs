using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoshinoLabs.IwaSync3
{
    public class IwaSync3 : IwaSync3Base
    {
#pragma warning disable CS0414
        // control
        [SerializeField]
        TrackMode defaultMode;
        [SerializeField]
        string defaultUrl;
        [SerializeField]
        bool allowSeeking = true;
        [SerializeField]
        bool defaultLoop = false;
        [SerializeField]
        float seekTimeSeconds = 10f;
        [SerializeField]
        string timeFormat = @"hh\:mm\:ss\:ff";

        // sync
        [SerializeField]
        float syncFrequency = 9.2f;
        [SerializeField]
        float syncThreshold = 0.92f;

        // error handling
        [SerializeField]
        [Range(0, 10)]
        int maxErrorRetry = 3;
        [SerializeField]
        [Range(10f, 30f)]
        float timeoutUnknownError = 10f;
        [SerializeField]
        [Range(6f, 30f)]
        float timeoutPlayerError = 6f;
        [SerializeField]
        [Range(6f, 30f)]
        float timeoutRateLimited = 6f;

        // lock
        [SerializeField]
        bool defaultLock = false;
        [SerializeField]
        bool allowInstanceOwner = true;

        // video
        [SerializeField]
        int maximumResolution = 720;

        // audio
        [SerializeField]
        bool defaultMute = false;
        [SerializeField]
        [Range(0f, 1f)]
        float defaultMinVolume = 0f;
        [SerializeField]
        [Range(0f, 1f)]
        float defaultMaxVolume = 0.5f;
        [SerializeField]
        [Range(0f, 1f)]
        float defaultVolume = 0.184f;

        // extra
        [SerializeField]
        [Tooltip("Low latency playback of live stream")]
        bool useLowLatency = false;
#pragma warning restore CS0414
    }
}
