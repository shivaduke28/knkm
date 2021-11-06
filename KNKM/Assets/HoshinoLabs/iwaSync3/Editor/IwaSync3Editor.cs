﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRC.SDK3.Video.Components;
using VRC.SDK3.Video.Components.AVPro;
using VRC.SDKBase;

namespace HoshinoLabs.IwaSync3
{
    [CustomEditor(typeof(IwaSync3))]
    public class IwaSync3Editor : IwaSync3EditorBase
    {
        IwaSync3 _target;

        SerializedProperty _defaultModeProperty;
        SerializedProperty _defaultUrlProperty;
        SerializedProperty _allowSeekingProperty;
        SerializedProperty _defaultLoopProperty;
        SerializedProperty _seekTimeSecondsProperty;
        SerializedProperty _timeFormatProperty;

        SerializedProperty _syncFrequencyProperty;
        SerializedProperty _syncThresholdProperty;

        SerializedProperty _maxErrorRetryProperty;
        SerializedProperty _timeoutUnknownErrorProperty;
        SerializedProperty _timeoutPlayerErrorProperty;
        SerializedProperty _timeoutRateLimitedProperty;

        SerializedProperty _defaultLockProperty;
        SerializedProperty _allowInstanceOwnerProperty;

        SerializedProperty _maximumResolutionProperty;

        SerializedProperty _defaultMuteProperty;
        SerializedProperty _defaultMinVolumeProperty;
        SerializedProperty _defaultMaxVolumeProperty;
        SerializedProperty _defaultVolumeProperty;

        SerializedProperty _useLowLatencyProperty;

        protected override void FindProperties()
        {
            base.FindProperties();

            _target = target as IwaSync3;

            _defaultModeProperty = serializedObject.FindProperty("defaultMode");
            _defaultUrlProperty = serializedObject.FindProperty("defaultUrl");
            _allowSeekingProperty = serializedObject.FindProperty("allowSeeking");
            _defaultLoopProperty = serializedObject.FindProperty("defaultLoop");
            _seekTimeSecondsProperty = serializedObject.FindProperty("seekTimeSeconds");
            _timeFormatProperty = serializedObject.FindProperty("timeFormat");

            _syncFrequencyProperty = serializedObject.FindProperty("syncFrequency");
            _syncThresholdProperty = serializedObject.FindProperty("syncThreshold");

            _maxErrorRetryProperty = serializedObject.FindProperty("maxErrorRetry");
            _timeoutUnknownErrorProperty = serializedObject.FindProperty("timeoutUnknownError");
            _timeoutPlayerErrorProperty = serializedObject.FindProperty("timeoutPlayerError");
            _timeoutRateLimitedProperty = serializedObject.FindProperty("timeoutRateLimited");

            _defaultLockProperty = serializedObject.FindProperty("defaultLock");
            _allowInstanceOwnerProperty = serializedObject.FindProperty("allowInstanceOwner");

            _maximumResolutionProperty = serializedObject.FindProperty("maximumResolution");

            _defaultMuteProperty = serializedObject.FindProperty("defaultMute");
            _defaultMinVolumeProperty = serializedObject.FindProperty("defaultMinVolume");
            _defaultMaxVolumeProperty = serializedObject.FindProperty("defaultMaxVolume");
            _defaultVolumeProperty = serializedObject.FindProperty("defaultVolume");

            _useLowLatencyProperty = serializedObject.FindProperty("useLowLatency");
        }

        public override void OnInspectorGUI()
        {
            FindProperties();

            base.OnInspectorGUI();

            serializedObject.Update();

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("Control", _italicStyle);
                EditorGUILayout.PropertyField(_defaultModeProperty);
                EditorGUILayout.PropertyField(_defaultUrlProperty);
                EditorGUILayout.PropertyField(_allowSeekingProperty);
                EditorGUILayout.PropertyField(_defaultLoopProperty);
                EditorGUILayout.PropertyField(_seekTimeSecondsProperty);
                EditorGUILayout.PropertyField(_timeFormatProperty);
            }

            EditorGUILayout.Space();

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("Sync", _italicStyle);
                EditorGUILayout.PropertyField(_syncFrequencyProperty);
                EditorGUILayout.PropertyField(_syncThresholdProperty);
            }

            EditorGUILayout.Space();

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("Error Handling", _italicStyle);
                EditorGUILayout.PropertyField(_maxErrorRetryProperty);
                EditorGUILayout.PropertyField(_timeoutUnknownErrorProperty);
                EditorGUILayout.PropertyField(_timeoutPlayerErrorProperty);
                EditorGUILayout.PropertyField(_timeoutRateLimitedProperty);
            }

            EditorGUILayout.Space();

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("Lock", _italicStyle);
                EditorGUILayout.PropertyField(_defaultLockProperty);
                EditorGUILayout.PropertyField(_allowInstanceOwnerProperty);
            }

            EditorGUILayout.Space();

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("Video", _italicStyle);
                EditorGUILayout.PropertyField(_maximumResolutionProperty);
            }

            EditorGUILayout.Space();

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("Audio", _italicStyle);
                EditorGUILayout.PropertyField(_defaultMuteProperty);
                EditorGUILayout.PropertyField(_defaultMinVolumeProperty);
                EditorGUILayout.PropertyField(_defaultMaxVolumeProperty);
                EditorGUILayout.PropertyField(_defaultVolumeProperty);
            }

            EditorGUILayout.Space();

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("Extra", _italicStyle);
                EditorGUILayout.PropertyField(_useLowLatencyProperty);
            }

            if (serializedObject.ApplyModifiedProperties())
                ApplyModifiedProperties();
        }

        public void ApplyModifiedProperties()
        {
            FindProperties();

            var core = _target.GetUdonComponentInChildren<Udon.IwaSync3.VideoCore>();
            if (core == null)
                return;
            core.SetPublicVariable("defaultMode", ((TrackMode)_defaultModeProperty.intValue).ToVideoCoreMode());
            core.SetPublicVariable("defaultUrl", new VRCUrl(_defaultUrlProperty.stringValue));
            core.SetPublicVariable("defaultLoop", _defaultLoopProperty.boolValue);
            core.SetPublicVariable("syncFrequency", _syncFrequencyProperty.floatValue);
            core.SetPublicVariable("syncThreshold", _syncThresholdProperty.floatValue);
            core.SetPublicVariable("maxErrorRetry", _maxErrorRetryProperty.intValue);
            core.SetPublicVariable("timeoutUnknownError", _timeoutUnknownErrorProperty.floatValue);
            core.SetPublicVariable("timeoutPlayerError", _timeoutPlayerErrorProperty.floatValue);
            core.SetPublicVariable("timeoutRateLimited", _timeoutRateLimitedProperty.floatValue);

            var speakers = FindObjectsOfType<Speaker>()
                .Where(x => x.mainIwaSync3 == _target)
                .Select(x => x.GetComponentInChildren<AudioSource>())
                .Distinct();

            var unityVideoPlayer = _target.GetComponentInChildren<VRCUnityVideoPlayer>();
            unityVideoPlayer.SetLoop(_defaultLoopProperty.boolValue);
            unityVideoPlayer.SetTargetAudioSources(speakers.ToArray());
            unityVideoPlayer.SetMaximumResolution(_maximumResolutionProperty.intValue);

            var avProVideoPlayer = _target.GetComponentInChildren<VRCAVProVideoPlayer>();
            avProVideoPlayer.SetLoop(_defaultLoopProperty.boolValue);
            avProVideoPlayer.SetMaximumResolution(_maximumResolutionProperty.intValue);
            avProVideoPlayer.SetUseLowLatency(_useLowLatencyProperty.boolValue);

            var controller = _target.GetUdonComponentInChildren<Udon.IwaSync3.VideoController>();
            if (controller == null)
                return;
            controller.SetPublicVariable("core", core);
            controller.SetPublicVariable("speaker", speakers.ToArray());
            controller.SetPublicVariable("defaultLock", _defaultLockProperty.boolValue);
            controller.SetPublicVariable("allowSeeking", _allowSeekingProperty.boolValue);
            controller.SetPublicVariable("seekTimeSeconds", _seekTimeSecondsProperty.floatValue);
            controller.SetPublicVariable("timeFormat", _timeFormatProperty.stringValue);
            controller.SetPublicVariable("allowInstanceOwner", _allowInstanceOwnerProperty.boolValue);
            controller.SetPublicVariable("defaultMute", _defaultMuteProperty.boolValue);
            controller.SetPublicVariable("defaultMinVolume", _defaultMinVolumeProperty.floatValue);
            controller.SetPublicVariable("defaultMaxVolume", _defaultMaxVolumeProperty.floatValue);
            controller.SetPublicVariable("defaultVolume", _defaultVolumeProperty.floatValue);

            var iwasync = _target.GetUdonComponentInChildren<Udon.IwaSync3.IwaSync3>();
            if (iwasync == null)
                return;
            iwasync.SetPublicVariable("core", core);
            iwasync.SetPublicVariable("controller", controller);
        }

    }
}
