using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDK3.Video.Components.AVPro;
using static VRC.SDK3.Video.Components.AVPro.VRCAVProVideoSpeaker;

namespace HoshinoLabs.IwaSync3
{
    [CustomEditor(typeof(Speaker))]
    public class SpeakerEditor : IwaSync3EditorBase
    {
        Speaker _target;

        SerializedProperty _iwaSync3Property;

        SerializedProperty _maxDistanceProperty;

        SerializedProperty _spatializeProperty;

        SerializedProperty _modeProperty;

        protected override void FindProperties()
        {
            base.FindProperties();

            _target = target as Speaker;

            _iwaSync3Property = serializedObject.FindProperty("iwaSync3");

            _maxDistanceProperty = serializedObject.FindProperty("maxDistance");

            _spatializeProperty = serializedObject.FindProperty("spatialize");

            _modeProperty = serializedObject.FindProperty("mode");
        }

        public override void OnInspectorGUI()
        {
            FindProperties();

            base.OnInspectorGUI();

            serializedObject.Update();

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("Main", _italicStyle);
                var iwaSync3 = GetMainIwaSync3(null);
                if (iwaSync3)
                    EditorGUILayout.LabelField(_iwaSync3Property.displayName, "Automatically set by Script");
                else
                    EditorGUILayout.PropertyField(_iwaSync3Property);
            }

            EditorGUILayout.Space();

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("Audio", _italicStyle);
                EditorGUILayout.PropertyField(_maxDistanceProperty);
            }

            EditorGUILayout.Space();

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("Spatialize", _italicStyle);
                EditorGUILayout.PropertyField(_spatializeProperty);
            }

            EditorGUILayout.Space();

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("Options", _italicStyle);
                EditorGUILayout.PropertyField(_modeProperty);
            }

            if (serializedObject.ApplyModifiedProperties())
                ApplyModifiedProperties();
        }

        public void ApplyModifiedProperties()
        {
            FindProperties();

            var iwaSync3 = GetMainIwaSync3(_iwaSync3Property);
            if (iwaSync3 == null)
                return;
            var core = iwaSync3.GetUdonComponentInChildren<Udon.IwaSync3.VideoCore>();

            var speaker = _target.GetComponentInChildren<AudioSource>();
            speaker.mute = new SerializedObject(iwaSync3).FindProperty("defaultMute").boolValue;
            speaker.volume = new SerializedObject(iwaSync3).FindProperty("defaultVolume").floatValue;
            speaker.maxDistance = _maxDistanceProperty.floatValue;

            var spatial = _target.GetComponentInChildren<VRCSpatialAudioSource>();
            spatial.EnableSpatialization = _spatializeProperty.boolValue;

            var avProVideoSpeaker = _target.GetComponentInChildren<VRCAVProVideoSpeaker>();
            avProVideoSpeaker.SetVideoPlayer(core.GetComponentInChildren<VRCAVProVideoPlayer>());
            avProVideoSpeaker.SetMode((ChannelMode)Enum.ToObject(typeof(ChannelMode), _modeProperty.enumValueIndex));
        }
    }
}
