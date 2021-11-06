﻿using System.Collections;
using System.Collections.Generic;
using UdonSharpEditor;
using UnityEditor;
using UnityEngine;

namespace HoshinoLabs.IwaSync3
{
    [CustomEditor(typeof(Queuelist))]
    public class QueuelistEditor : IwaSync3EditorBase
    {
        Queuelist _target;

        SerializedProperty _iwaSync3Property;

        protected override void FindProperties()
        {
            base.FindProperties();

            _target = target as Queuelist;

            _iwaSync3Property = serializedObject.FindProperty("iwaSync3");
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
            var controller = iwaSync3.GetUdonComponentInChildren<Udon.IwaSync3.VideoController>();

            var self = _target.GetUdonComponentInChildren<Udon.IwaSync3.Queuelist>();
            self.SetPublicVariable("core", core);
            self.SetPublicVariable("controller", controller);
        }
    }
}
