using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HoshinoLabs.IwaSync3
{
    public abstract class IwaSync3EditorBase : Editor
    {
        Texture2D _splash;

        protected GUIStyle _headerTitleStyle;
        protected GUIStyle _headerVersionStyle;
        protected GUIStyle _italicStyle;

        protected virtual void FindProperties()
        {
            _splash = Resources.Load<Texture2D>($"{Udon.IwaSync3.IwaSync3.APP_NAME}_Splash");

            _headerTitleStyle = new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Italic,
                fontSize = 14,
            };
            _headerTitleStyle.normal.textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black;
            _headerVersionStyle = new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Italic,
            };
            _headerVersionStyle.normal.textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black;
            _italicStyle = new GUIStyle()
            {
                fontStyle = FontStyle.Italic,
            };
            _italicStyle.normal.textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black;
        }

        void OnInspectorHeader()
        {
            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                GUILayout.Label($"{Udon.IwaSync3.IwaSync3.APP_NAME}", _headerTitleStyle);
                GUILayout.Label($"{Udon.IwaSync3.IwaSync3.APP_VERSION}", _headerVersionStyle);
            }
        }

        void OnInspectorSplash()
        {
            var rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(_splash.height));
            rect.xMin = Mathf.Max((rect.width - _splash.width + 18f) / 2f, 0f);
            rect.width = _splash.width;
            rect.height = _splash.height;
            GUI.DrawTexture(rect, _splash, ScaleMode.ScaleToFit, true, 0f);
        }

        public override void OnInspectorGUI()
        {
            OnInspectorHeader();
            EditorGUILayout.Space();

            OnInspectorSplash();
            EditorGUILayout.Space();
        }

        protected IwaSync3 GetMainIwaSync3(SerializedProperty property)
        {
            var iwaSync3 = ((Component)target).GetComponentInParent<IwaSync3>();
            if (iwaSync3)
                return iwaSync3;
            if (FindObjectsOfType<IwaSync3>().Length == 1)
                return FindObjectOfType<IwaSync3>();
            return property == null ? null : (IwaSync3)property.objectReferenceValue;
        }
    }
}
